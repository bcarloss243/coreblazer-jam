using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class GumboPotInteraction : MonoBehaviour, IInteractable
{
    [Header("Pot UI")]
    public UIDocument potUIDocument;

    private VisualElement _root;
    private Label _statusLabel;
    private Label _reactionLabel;
    private Button _cookBtn, _addBtn, _closeBtn;

    public float InteractionPriority => -1f;

    void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        _root           = potUIDocument.rootVisualElement;
        _statusLabel    = _root.Q<Label>("StatusLabel");
        _reactionLabel  = _root.Q<Label>("ReactionLabel");
        _cookBtn        = _root.Q<Button>("CookButton");
        _addBtn         = _root.Q<Button>("AddButton");
        _closeBtn       = _root.Q<Button>("CloseButton");

        _cookBtn.clicked  += TryCookGumbo;
        _addBtn.clicked   += TryAddIngredient;
        _closeBtn.clicked += () => _root.style.display = DisplayStyle.None;

        _reactionLabel.style.display = DisplayStyle.None;
        _root.style.display = DisplayStyle.None;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            InteractionManager.Instance.Register(this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            InteractionManager.Instance.Unregister(this);
    }

    public void OnInteract()
    {
        _statusLabel.text = "Ready to cook!";
        _reactionLabel.text = "";
        _reactionLabel.style.display = DisplayStyle.None;

        _addBtn.style.display = PlayerInventory.Instance.HasIngredients()
            ? DisplayStyle.Flex : DisplayStyle.None;

        _root.style.display = DisplayStyle.Flex;
    }

    void TryAddIngredient()
    {
        var ingredient = PlayerInventory.Instance.PopNextIngredient();
        if (ingredient == null)
        {
            _statusLabel.text = "No ingredients in your inventory.";
            return;
        }

        GameManager.Instance.AddIngredient(ingredient, bonus: false);
        _statusLabel.text = $"{ingredient} added to the pot.";
        ShowReaction(GameManager.Instance.GetIngredientReaction(ingredient, false));

        _addBtn.style.display = PlayerInventory.Instance.HasIngredients()
            ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void TryCookGumbo()
    {
        bool success = GameManager.Instance.CookGumbo(out string feedback);
        _statusLabel.text = feedback;
    }

    void ShowReaction(string text)
    {
        _reactionLabel.text = text;
        _reactionLabel.style.display = DisplayStyle.Flex;
        StartCoroutine(HideReactionAfter(2f));
    }

    IEnumerator HideReactionAfter(float sec)
    {
        yield return new WaitForSeconds(sec);
        _reactionLabel.style.display = DisplayStyle.None;
    }
}
