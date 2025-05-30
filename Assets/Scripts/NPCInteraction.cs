using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCInteraction : MonoBehaviour, IInteractable
{
    [Header("NPC Data & Ingredient")]
    public NPCData data;
    public GameObject ingredientPrefab;

    private bool _memoryShared = false;
    private bool _bonusGiven = false;

    public float InteractionPriority => 0f;

    void Awake()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"Entered trigger of {gameObject.name} with: {col.gameObject.name}");

        if (col.CompareTag("Player"))
        {
            Debug.Log($"Registering interactable: {gameObject.name}");
            InteractionManager.Instance.Register(this);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            InteractionManager.Instance.Unregister(this);
        }
    }

    public void OnInteract()
    {
        Debug.Log($"Interacted with {data.npcName}");

        var ui = DialogueUIController.Instance;
        ui.ClearUI();
        ui.ShowPanel();

        ui.SetDialogueText("What would you like to do?");
        ui.ShowBackstory(data.backstory);
        ui.SetPortrait(data.portrait);

        ui.SetButtonsActive(!_memoryShared, !_memoryShared, true, true);

        ui.shareMemoryButton.onClick.RemoveAllListeners();
        ui.askIngredientButton.onClick.RemoveAllListeners();
        ui.offerGumboButton.onClick.RemoveAllListeners();
        ui.exitButton.onClick.RemoveAllListeners();

        ui.shareMemoryButton.onClick.AddListener(OnShareMemory);
        ui.askIngredientButton.onClick.AddListener(OnAskForIngredient);
        ui.offerGumboButton.onClick.AddListener(OnOfferGumbo);
        ui.exitButton.onClick.AddListener(EndInteraction);
    }

    void OnShareMemory()
    {
        if (!_memoryShared)
        {
            _memoryShared = true;
            SpawnIngredient(data.ingredientType, 1);
        }

        var ui = DialogueUIController.Instance;
        ui.SetButtonsActive(false, false, true, true);
        ui.SetDialogueText(data.memoryText);
        StartCoroutine(DelayedQuiz());
    }

    IEnumerator DelayedQuiz()
    {
        yield return new WaitForSeconds(4.5f);

        DialogueUIController.Instance.ShowQuestion(
            data.question,
            data.answers,
            OnAnswerSelected
        );
    }

    void OnAnswerSelected(int index)
    {
        bool correct = index == data.correctAnswerIndex;

        if (correct && !_bonusGiven)
        {
            _bonusGiven = true;
            SpawnIngredient(data.bonusIngredientType, 1);
            DialogueUIController.Instance.SetDialogueText(
                "You really listened—and that means the world to me.\n\n(A subtle spice joins the pot.)"
            );
        }
        else
        {
            DialogueUIController.Instance.SetDialogueText(
                "That's not quite right, but thanks for listening."
            );
        }
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

        DialogueUIController.Instance.SetDialogueText(
            lines[Random.Range(0, lines.Length)]
        );
    }

    void OnOfferGumbo()
    {
        int have = GameManager.Instance.GetIngredientCount(data.ingredientType);

        string line = GameManager.Instance.HasAllCoreIngredients()
            ? "Just like my mother used to make..."
            : (have >= data.requiredIngredientCount
                ? "Pretty decent gumbo. Not bad for your first time!"
                : "Tastes a little watery... I think it’s missing something.");

        DialogueUIController.Instance.SetDialogueText(line);
    }

    void EndInteraction()
    {
        DialogueUIController.Instance.HidePanel();
    }

    void SpawnIngredient(string ingredient, int count)
    {
        if (!ingredientPrefab)
        {
            Debug.LogError("Ingredient prefab missing!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new(Random.Range(-0.3f, 0.3f), 1.5f + i * 0.3f, 0);
            GameObject go = Instantiate(ingredientPrefab, transform.position + offset, Quaternion.identity);
            if (go.TryGetComponent(out IngredientPickup pickup))
                pickup.Init(ingredient);
        }
    }
}
