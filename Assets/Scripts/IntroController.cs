// Assets/Scripts/IntroController.cs
using UnityEngine;

/// <summary>
/// • Runs once when the scene loads.  
/// • Disables player movement, shows Nina’s opening monologue,  
///   then waits for the player to press the “Leave / Close” button.  
/// • Hands control back to the player and tells <see cref="SpawnManager"/>
///   to begin the tutorial pick-ups.
/// </summary>
public class IntroController : MonoBehaviour
{
    [TextArea(3,10)]
    [Tooltip("First lines Nina says when the game starts.")]
    public string introText =
        "I’ve come to Grandma Bev’s old neighbourhood…\n" +
        "Found a faded gumbo recipe on a napkin. I’ve got roux, chicken stock, and rice—" +
        "but I’ll need to track down the rest…";

    void Start()
    {
        // ── 1. Freeze the player (safely – if we’re in a test scene w/o PlayerMover, skip) ──
        if (PlayerMover.Instance) PlayerMover.Instance.enabled = false;

        // ── 2. Bring up the dialogue panel ──
        var ui = DialogueUIController.Instance;
        ui.ClearUI();
        ui.SetDialogueText(introText);
        ui.ShowBackstory("");                    // hide / clear the back-story box
        ui.ShowPanel();

        // Only the “Leave / Close” button should be visible
        ui.SetButtonsActive(share:false, ask:false, offer:false, exit:true);

        // ── 3. Wire the exit button exactly once ──
        ui.exitButton.onClick.RemoveAllListeners();
        ui.exitButton.onClick.AddListener(() =>
        {
            ui.HidePanel();

            // hand control back
            if (PlayerMover.Instance) PlayerMover.Instance.enabled = true;

            // kick-off tutorial flow
            FindObjectOfType<SpawnManager>()?.BeginTutorial();
        });
    }
}

