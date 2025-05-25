using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralizes all IInteractable Space-key events.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    // All interactables currently in trigger range
    private readonly List<IInteractable> _inRange = new List<IInteractable>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else                            Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _inRange.Count > 0)
        {
            // sort descending by priority
            _inRange.Sort((a, b) => b.InteractionPriority.CompareTo(a.InteractionPriority));
            _inRange[0].OnInteract();
        }
    }

    public void Register(IInteractable i)
    {
        if (!_inRange.Contains(i))
            _inRange.Add(i);
    }

    public void Unregister(IInteractable i)
    {
        _inRange.Remove(i);
    }
}
