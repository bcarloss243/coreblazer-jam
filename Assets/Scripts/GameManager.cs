using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Perfect Gumbo Recipe")]
    [Tooltip("List the main ingredientType of each of your 5 NPCs here")]
    public List<string> coreIngredients = new List<string>();

    private HashSet<string> ingredients = new HashSet<string>();
    private int gumboServings = 5; // starting servings
    private const int MAX_SERVINGS = 10;
    private bool gumboFinished = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // TEMP TEST for UI Wiring
        DialogueUIController.Instance.SetDialogueText("A soft jazz melody sways in the distance...");
        DialogueUIController.Instance.ShowBackstory("They say her gumbo could raise the dead. Or at least make them smile.");
        DialogueUIController.Instance.ShowQuestion(
            "What’s your name, sugar?",
            new string[] { "Nina", "Lucien", "Remy" },
            (int choice) =>
            {
                DialogueUIController.Instance.SetDialogueText("You picked: " + choice);
            });

        DialogueUIController.Instance.SetButtonsActive(true, true, true, true);
    }

    // — Ingredient Management —

    public void AddIngredient(string ingredient, bool bonus)
    {
        bool wasNew = ingredients.Add(ingredient);
        if (wasNew)
        {
            int toAdd = bonus ? 2 : 1;
            gumboServings = Mathf.Min(gumboServings + toAdd, MAX_SERVINGS);
            Debug.Log($"New ingredient added: {ingredient}. Bonus? {bonus}. Servings: {gumboServings}/{MAX_SERVINGS}");
        }
        else
        {
            Debug.Log($"{ingredient} was already in the pot—no extra servings added.");
        }
    }

    public void RemoveIngredient(string ingredient)
    {
        if (ingredients.Remove(ingredient))
            Debug.Log($"Removed {ingredient} from the pot.");
    }

    public int GetRemainingServings() => gumboServings;

    public void EatFromPot()
    {
        if (gumboServings > 0)
        {
            gumboServings--;
            Debug.Log($"Nina ate a serving! Servings left: {gumboServings}");
        }
        else
        {
            Debug.Log("The gumbo pot is empty—no servings left!");
        }
    }

    // — Perfect Gumbo Check & Cooking —

    public bool HasAllCoreIngredients()
    {
        foreach (var req in coreIngredients)
            if (!ingredients.Contains(req))
                return false;
        return true;
    }

    public bool CookGumbo(out string feedback)
    {
        if (!HasAllCoreIngredients())
        {
            feedback = "You’re missing some ingredients!";
            return false;
        }

        foreach (var req in coreIngredients)
            RemoveIngredient(req);

        feedback = "Perfect gumbo! You did it!";
        gumboFinished = true;
        return true;
    }

    public bool IsGumboFinished() => gumboFinished;

    // — NPC “Give Me Ingredient” Helpers —

    public bool TryGiveIngredient(string ingredient, out string feedback)
    {
        if (ingredients.Contains(ingredient))
        {
            feedback = "Hey, that’s rude—you already have that from me.";
            return false;
        }

        AddIngredient(ingredient, bonus: false);
        feedback = $"Here you go—one {ingredient} coming right up.";
        return true;
    }

    public int GetIngredientCount(string ingredient) =>
        ingredients.Contains(ingredient) ? 1 : 0;

    // — Ingredient Reactions —

    public string GetIngredientReaction(string ingredient, bool bonus)
    {
        if (bonus)
        {
            // Generic message for spices or extra ingredients
            return $"{ingredient} joins the gumbo.";
        }

        // Poetic lines for core ingredients
        return ingredient switch
        {
            "celery"      => "Celery drops in with a soft sizzle.",
            "andouille"   => "The andouille makes the broth bubble joyfully.",
            "bell pepper" => "Bell pepper dissolves into light — like a memory she didn’t know she had.",
            "onion"       => "Onion melts into the broth with a quiet sweetness.",
            "chicken"     => "The chicken settles in, grounding everything with warmth.",
            _             => $"You added {ingredient} to the pot."
        };
    }
}
