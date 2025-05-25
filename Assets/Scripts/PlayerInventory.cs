using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    // Ingredients the player has picked up but not yet added to the pot
    private Queue<string> _heldIngredients = new Queue<string>();

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Called by IngredientPickup when the player presses Space on a pickup.
    /// </summary>
    public void CollectIngredient(string ingredientName)
    {
        _heldIngredients.Enqueue(ingredientName);
        Debug.Log($"Collected “{ingredientName}”. {_heldIngredients.Count} now held.");
    }

    /// <summary>
    /// True if there’s at least one ingredient waiting to be added.
    /// </summary>
    public bool HasIngredients()
    {
        return _heldIngredients.Count > 0;
    }

    /// <summary>
    /// Remove and return the next ingredient the player is holding,
    /// or null if there are none.
    /// </summary>
    public string PopNextIngredient()
    {
        if (_heldIngredients.Count == 0)
            return null;

        var next = _heldIngredients.Dequeue();
        Debug.Log($"Removed “{next}” from inventory. {_heldIngredients.Count} left.");
        return next;
    }

    /// <summary>
    /// (Optional) Peek at all held ingredients for UI/debugging.
    /// </summary>
    public List<string> PeekAllHeld()
    {
        return new List<string>(_heldIngredients);
    }

    /// <summary>
    /// (Optional) Clear all held ingredients.
    /// </summary>
    public void ClearHeldIngredients()
    {
        _heldIngredients.Clear();
        Debug.Log("Cleared all held ingredients.");
    }
}
