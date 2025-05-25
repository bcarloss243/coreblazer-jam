using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "Gumbo/NPC Data")]
public class NPCData : ScriptableObject
{
    [Header("Identity")]
    public string npcName;
    public Sprite portrait;

    [Header("Backstory & Quiz")]
    [TextArea] 
    public string backstory;
    public string question;
    public string[] answers = new string[3];
    public int correctAnswerIndex;

    [Header("Gumbo Ingredients")]
    [Tooltip("The main ingredient you always get when sharing this NPC's memory")]
    public string ingredientType;

    [Tooltip("An extra (bonus) ingredient if you answer the quiz correctly")]
    public string bonusIngredientType;

    [Tooltip("How many of this ingredient must already be in the pot before this NPC thinks it's perfect")]
    public int requiredIngredientCount = 5;
}

