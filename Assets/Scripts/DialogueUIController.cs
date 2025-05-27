using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    public static DialogueUIController Instance;

    [Header("Text Elements")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI backstoryText;

    [Header("Portrait")]
    public Image portraitImage;

    [Header("Quiz Buttons")]
    public GameObject choiceGroup;
    public Button choiceAButton;
    public Button choiceBButton;
    public Button choiceCButton;

    [Header("Action Buttons")]
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

        if (choiceAButton) choiceAButton.onClick.AddListener(() => HandleChoice(0));
        if (choiceBButton) choiceBButton.onClick.AddListener(() => HandleChoice(1));
        if (choiceCButton) choiceCButton.onClick.AddListener(() => HandleChoice(2));
        if (exitButton) exitButton.onClick.AddListener(HidePanel);

        choiceGroup?.SetActive(false);
        gameObject.SetActive(false);
    }

    // ─────────────────────── Public API ───────────────────────

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        ClearUI();
        gameObject.SetActive(false);
    }

    public void SetDialogueText(string text)
    {
        if (dialogueText != null)
            dialogueText.text = text;
    }

    public void SetQuestionText(string text)
    {
        if (questionText != null)
            questionText.text = text;
    }

    public void ShowBackstory(string backstory)
    {
        if (backstoryText != null)
        {
            backstoryText.text = backstory;
            backstoryText.gameObject.SetActive(true);
        }
    }

    public void SetPortrait(Sprite sprite)
    {
        if (portraitImage != null)
        {
            portraitImage.sprite = sprite;
            portraitImage.gameObject.SetActive(sprite != null);
        }
    }

    public void ShowQuestion(string question, string[] options, System.Action<int> callback)
    {
        SetQuestionText(question);
        onChoiceSelected = callback;

        if (options.Length >= 3)
        {
            choiceAButton.GetComponentInChildren<TextMeshProUGUI>().text = options[0];
            choiceBButton.GetComponentInChildren<TextMeshProUGUI>().text = options[1];
            choiceCButton.GetComponentInChildren<TextMeshProUGUI>().text = options[2];
        }

        choiceGroup?.SetActive(true);
    }

    public void HideQuestion()
    {
        choiceGroup?.SetActive(false);
    }

    public void SetButtonsActive(bool share, bool ask, bool offer, bool exit)
    {
        shareMemoryButton?.gameObject.SetActive(share);
        askIngredientButton?.gameObject.SetActive(ask);
        offerGumboButton?.gameObject.SetActive(offer);
        exitButton?.gameObject.SetActive(exit);
    }

    public void ClearUI()
    {
        if (dialogueText) dialogueText.text = "";
        if (questionText) questionText.text = "";
        if (backstoryText)
        {
            backstoryText.text = "";
            backstoryText.gameObject.SetActive(false);
        }

        SetPortrait(null);
        HideQuestion();
        SetButtonsActive(false, false, false, false);
    }

    // ─────────────────────── Internal Logic ───────────────────────

    private void HandleChoice(int index)
    {
        HideQuestion();
        onChoiceSelected?.Invoke(index);
    }
}
