using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    public static DialogueUIController Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI backstoryText;

    public GameObject choiceGroup;
    public Button choiceAButton;
    public Button choiceBButton;
    public Button choiceCButton;

    public Button shareMemoryButton;
    public Button askIngredientButton;
    public Button offerGumboButton;
    public Button exitButton;

    private System.Action<int> onChoiceSelected;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Hook up quiz buttons
        choiceAButton.onClick.AddListener(() => HandleChoice(0));
        choiceBButton.onClick.AddListener(() => HandleChoice(1));
        choiceCButton.onClick.AddListener(() => HandleChoice(2));

        // Initially hide quiz group
        choiceGroup.SetActive(false);
    }

    // ────────────────────────────── Public API ──────────────────────────────

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ShowBackstory(string backstory)
    {
        backstoryText.text = backstory;
        backstoryText.gameObject.SetActive(true);
    }

    public void ShowQuestion(string question, string[] options, System.Action<int> callback)
    {
        questionText.text = question;
        onChoiceSelected = callback;

        choiceAButton.GetComponentInChildren<TextMeshProUGUI>().text = options[0];
        choiceBButton.GetComponentInChildren<TextMeshProUGUI>().text = options[1];
        choiceCButton.GetComponentInChildren<TextMeshProUGUI>().text = options[2];

        choiceGroup.SetActive(true);
    }

    public void HideQuestion()
    {
        choiceGroup.SetActive(false);
    }

    public void SetButtonsActive(bool share, bool ask, bool offer, bool exit)
    {
        shareMemoryButton.gameObject.SetActive(share);
        askIngredientButton.gameObject.SetActive(ask);
        offerGumboButton.gameObject.SetActive(offer);
        exitButton.gameObject.SetActive(exit);
    }

    public void ClearUI()
    {
        dialogueText.text = "";
        backstoryText.text = "";
        questionText.text = "";
        HideQuestion();
    }

    // ────────────────────────────── Internal Logic ──────────────────────────────

    private void HandleChoice(int index)
    {
        HideQuestion();
        onChoiceSelected?.Invoke(index);
    }
}
