using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("References")]
    public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [Header("Choice Buttons")]
    public Button choiceButton1;
    public Button choiceButton2;
    public Button choiceButton3;

    private void Awake()
    {
        // Singleton pattern so other scripts can easily call DialogueUI.Instance.Show()
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Show(NPCData data)
    {
        gameObject.SetActive(true); // enable the dialogue box

        portraitImage.sprite = data.portrait;
        nameText.text = data.npcName;
        dialogueText.text = data.backstory;

        // Set up quiz choices
        choiceButton1.GetComponentInChildren<TMP_Text>().text = data.answers[0];
        choiceButton2.GetComponentInChildren<TMP_Text>().text = data.answers[1];
        choiceButton3.GetComponentInChildren<TMP_Text>().text = data.answers[2];

        // TODO: Add logic for button onClick here
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
