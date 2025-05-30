using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IngredientPickup : MonoBehaviour, IInteractable
{
    [Tooltip("The key this pickup will register when collected")]
    public string ingredientName;

    public float InteractionPriority => 1f;

    public void Init(string name)
    {
        ingredientName = name;
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
        GameManager.Instance.AddIngredient(ingredientName, false);
        Destroy(gameObject);
    }
}
