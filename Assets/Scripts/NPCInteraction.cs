using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("Data & UI Toolkit Document")]
    public NPCData    data;        // unique ScriptableObject per NPC
    public UIDocument uiDocument;  // DialoguePanel.uxml instance

    [Header("Ingredient Prefab")]
    public GameObject ingredientPrefab;

    // UI refs
    VisualElement _root, _actionBox, _choiceBox;
    Label         _nameLabel, _dialogueLabel, _questionText;
    Label         _backstoryLabel;
    Image         _portraitImage;
    Button        _shareBtn, _askBtn, _offerBtn, _exitBtn;
    Button[]      _choiceButtons;

    // state flags
    bool _memoryShared;  // main ingredient given?
    bool _bonusGiven;    // bonus spice given?

    // NPC priority: after pickups (1), before pot (-1)
    public float InteractionPriority => 0f;

    void Awake()
    {
        // make collider a trigger
        var bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;

        // grab UI elements
        _root           = uiDocument.rootVisualElement;
        _nameLabel      = _root.Q<Label>("Name");
        _dialogueLabel  = _root.Q<Label>("DialogueText");
        _backstoryLabel = _root.Q<Label>("BackstoryLabel");
        _portraitImage  = _root.Q<Image>("Portrait");

        _actionBox      = _root.Q<VisualElement>("ActionBox");
        _choiceBox      = _root.Q<VisualElement>("ChoiceBox");

        _shareBtn       = _root.Q<Button>("ShareBtn");
        _askBtn         = _root.Q<Button>("AskBtn");
        _offerBtn       = _root.Q<Button>("OfferBtn");
        _exitBtn        = _root.Q<Button>("ExitBtn");

        // wire buttons
        _shareBtn.clicked += OnShareMemory;
        _askBtn.clicked   += OnAskForIngredient;
        _offerBtn.clicked += OnOfferGumbo;
        _exitBtn.clicked  += EndInteraction;

        // quiz buttons
        var choices = _root.Query<Button>(className: "choice-button").ToList();
        _choiceButtons = choices.ToArray();
        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            int idx = i;
            _choiceButtons[i].clicked += () => OnChoice(idx);
        }

        // hide all panels initially
        _root.style.display          = DisplayStyle.None;
        _choiceBox.style.display     = DisplayStyle.None;
        _exitBtn.style.display       = DisplayStyle.None;
        _backstoryLabel.style.display = DisplayStyle.None;
    }

    #region IInteractable Registration
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            InteractionManager.Instance.Register(this);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            InteractionManager.Instance.Unregister(this);
    }

    public void OnInteract()
    {
        if (_root.style.display == DisplayStyle.None)
            OpenPanel();
    }
    #endregion

    void OpenPanel()
    {
        _nameLabel.text       = data.npcName;
        _portraitImage.sprite = data.portrait;
        _dialogueLabel.text   = "What would you like to do?";

        _actionBox.style.display     = DisplayStyle.Flex;
        _choiceBox.style.display     = DisplayStyle.None;
        _exitBtn.style.display       = DisplayStyle.Flex;
        _backstoryLabel.style.display = DisplayStyle.None;

        _shareBtn.style.display = _memoryShared ? DisplayStyle.None : DisplayStyle.Flex;
        _askBtn.style.display   = _memoryShared ? DisplayStyle.None : DisplayStyle.Flex;

        _root.style.display = DisplayStyle.Flex;
    }

    void OnShareMemory()
    {
        if (!_memoryShared)
        {
            _memoryShared = true;
            SpawnIngredient(data.ingredientType, 1);
        }

        _shareBtn.style.display = DisplayStyle.None;
        _askBtn.style.display   = DisplayStyle.None;

        _backstoryLabel.text          = data.backstory;
        _backstoryLabel.style.display = DisplayStyle.Flex;

        _dialogueLabel.text = ""; // clear main box
        StartCoroutine(TransitionToQuiz());
    }

    IEnumerator TransitionToQuiz()
    {
        yield return new WaitForSeconds(2.5f);

        _actionBox.style.display = DisplayStyle.None;
        _choiceBox.style.display = DisplayStyle.Flex;

        _questionText = _root.Q<Label>("QuestionText");
        _questionText.text = data.question;
        for (int i = 0; i < _choiceButtons.Length; i++)
            _choiceButtons[i].text = data.answers[i];
    }

    void OnChoice(int idx)
    {
        bool correct = idx == data.correctAnswerIndex;

        if (correct && !_bonusGiven)
        {
            _bonusGiven = true;
            SpawnIngredient(data.bonusIngredientType, 1);
        }

        _dialogueLabel.text = correct
            ? "You really listened—and that means the world to me."
            : "That's not quite right, but thanks for listening.";

        _actionBox.style.display = DisplayStyle.None;
        _choiceBox.style.display = DisplayStyle.None;
        _exitBtn.style.display   = DisplayStyle.Flex;
    }

    void OnAskForIngredient()
    {
        string[] lines = {
            "No, I don’t think I have that.",
            "Come back later.",
            "I don’t know who you are.",
            "Ask me again when you’ve earned it.",
            "Hmph. Not everyone gets a taste."
        };
        _dialogueLabel.text = lines[Random.Range(0, lines.Length)];
    }

    void OnOfferGumbo()
    {
        int have = GameManager.Instance.GetIngredientCount(data.ingredientType);

        if (GameManager.Instance.HasAllCoreIngredients())
            _dialogueLabel.text = "Just like my mother used to make...";
        else if (have >= data.requiredIngredientCount)
            _dialogueLabel.text = "Pretty decent gumbo. Not bad for your first time!";
        else
            _dialogueLabel.text = "Tastes a little watery... I think it’s missing something.";
    }

    void EndInteraction()
    {
        _root.style.display          = DisplayStyle.None;
        _backstoryLabel.style.display = DisplayStyle.None;
    }

    void SpawnIngredient(string ing, int count)
    {
        if (ingredientPrefab == null)
        {
            Debug.LogError("Ingredient prefab missing!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new(Random.Range(-0.3f, 0.3f), 1.5f + i * 0.3f, 0);
            var go = Instantiate(ingredientPrefab, transform.position + offset, Quaternion.identity);
            if (go.TryGetComponent(out IngredientPickup pickup))
                pickup.Init(ing);
        }
    }
}
