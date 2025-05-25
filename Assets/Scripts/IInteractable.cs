/// <summary>
/// Anything the player can “use” (NPCs, pickups, etc.) implements this.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called by InteractionManager when the player presses the interact key
    /// and this object has the highest priority.
    /// </summary>
    void OnInteract();

    /// <summary>
    /// Higher values == “closer” / more important.
    /// NPCs can be 0, pickups can be e.g. 1.
    /// </summary>
    float InteractionPriority { get; }
}
