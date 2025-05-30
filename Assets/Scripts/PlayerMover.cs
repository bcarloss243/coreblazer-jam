using UnityEngine;

/// <summary>
/// Very small top-down mover + optional singleton so other scripts
/// can safely enable/disable player control.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMover : MonoBehaviour
{
    // ───────── singleton access ─────────
    public static PlayerMover Instance { get; private set; }

    [Header("Movement")]
    public float speed = 3f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (!enabled) return;  // IntroController disables us at start

        var move = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        transform.Translate(move * speed * Time.deltaTime);
    }
}
