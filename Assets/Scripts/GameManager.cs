using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Owns the global ingredient list, servings counter, and win-condition checks.
/// Fires events when a *new* core ingredient is added or when a perfect gumbo is cooked.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Perfect Gumbo Recipe (main ingredients only)")]
    [Tooltip("Populate this with the main ingredientType of each NPC *in the Inspector*.")]
    public List<string> coreIngredients = new List<string>();

    private readonly HashSet<string> _ingredients = new();
    private int _servings = 5;
    private const int MAX_SERVINGS = 10;
    private bool _gumboFinished;

    public UnityEvent<string> OnNewCoreIngredient = new();
    public UnityEvent OnPerfectGumbo = new();
    public UnityEvent OnTutorialComplete = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AddIngredient(string ingredient, bool bonus)
    {
        bool isNew = _ingredients.Add(ingredient);
        if (isNew)
        {
            int delta = bonus ? 2 : 1;
            _servings = Mathf.Min(_servings + delta, MAX_SERVINGS);
            Debug.Log($"Added {ingredient}. Bonus? {bonus}. Servings now {_servings}/{MAX_SERVINGS}");

            if (coreIngredients.Contains(ingredient))
                OnNewCoreIngredient.Invoke(ingredient);

            // check for tutorial completion
            if (HasTutorialIngredients())
                OnTutorialComplete.Invoke();
        }
        else
        {
            Debug.Log($"{ingredient} already in pot – no changes.");
        }
        return isNew;
    }

    public void RemoveIngredient(string ingredient) => _ingredients.Remove(ingredient);

    public bool HasIngredient(string ingredient) => _ingredients.Contains(ingredient);

    public int GetIngredientCount(string ingredient) => _ingredients.Contains(ingredient) ? 1 : 0;

    public int RemainingServings => _servings;

    public void EatServing()
    {
        if (_servings <= 0) { Debug.Log("Pot is empty."); return; }
        _servings--;
        Debug.Log($"Nina ate a bowl. {_servings} left.");
    }

    public bool HasAllCoreIngredients()
    {
        foreach (string req in coreIngredients)
            if (!_ingredients.Contains(req)) return false;
        return true;
    }

    public int CountCoreIngredients()
    {
        int count = 0;
        foreach (string req in coreIngredients)
            if (_ingredients.Contains(req)) count++;
        return count;
    }

    public bool HasTutorialIngredients()
    {
        return _ingredients.Contains("roux") && _ingredients.Contains("rice") && _ingredients.Contains("stock");
    }

    public bool CookGumbo(out string feedback)
    {
        if (_gumboFinished)
        {
            feedback = "Gumbo is already perfect!";
            return false;
        }

        if (!HasAllCoreIngredients())
        {
            feedback = "Still missing ingredients.";
            return false;
        }

        foreach (string req in coreIngredients)
            _ingredients.Remove(req);

        _gumboFinished = true;
        feedback = "Perfect gumbo!";
        OnPerfectGumbo.Invoke();
        return true;
    }

    public bool GumboFinished => _gumboFinished;

    public string GetIngredientReaction(string ingredient, bool bonus)
    {
        if (bonus) return $"{ingredient} sprinkles into the pot with a sparkle.";

        return ingredient switch
        {
            "celery"      => "Celery drops in with a soft sizzle.",
            "andouille"   => "The andouille wakes the broth, crackling with smoke.",
            "bell pepper" => "Bell pepper melts into light—like an old memory.",
            "onion"       => "Onion softens, sweet and sure.",
            "chicken"     => "Chicken settles in, grounding the whole brew.",
            _             => $"{ingredient} slips into the gumbo."
        };
    }
}
