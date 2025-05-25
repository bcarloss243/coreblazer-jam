using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IngredientPickup : MonoBehaviour, IInteractable
{
    [Tooltip("The key this pickup will register when collected")]
    public string ingredientName;

    // pickups > NPCInteraction in priority
    public float InteractionPriority => 1f;

    /// <summary>
    /// Call right after Instantiate() to set which ingredient this is.
    /// </summary>
    public void Init(string name)
    {
        ingredientName = name;
    }

    #region Trigger registration
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
    #endregion

    /// <summary>
    /// Called by InteractionManager when the player presses Space and this has highest priority.
    /// </summary>
    public void OnInteract()
    {
        PlayerInventory.Instance.CollectIngredient(ingredientName);
        Destroy(gameObject);
    }
}
