using System.Collections.Generic;
using UnityEngine;

public enum FlowState
{
    Tutorial,   // player picks up roux / stock / rice
    Wave1,      // Beau–Jules–Lalainne wander
    Mireille,   // Mireille arrives
    HenriDoor,  // player triggers Henri’s door → Henri spawns
    Grandma,    // perfect-gumbo cooked, Grandma Bev appears
    Sharing,
    Done
}

/// <summary>
/// Listens to GameManager events and instantiates NPC prefabs at the right moments.
/// All “business logic” (how many core ingredients trigger the next wave, etc.)
/// lives here so NPC prefabs stay dumb.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs & Spawn Points")]
    [Tooltip("Beau, Jules, Lalainne – shown after tutorial pick-ups")]
    public List<GameObject> wave1Prefabs;

    public GameObject mireillePrefab;
    public GameObject henriPrefab;
    public GameObject grandmaPrefab;

    public Transform[] spawnPoints;     // randomised drop-in spots

    private FlowState _state = FlowState.Tutorial;
    private bool _wave1Spawned = false;
    private bool _mireilleSpawned = false;

    void Awake()
    {
        GameManager.Instance.OnNewCoreIngredient.AddListener(HandleCoreIngredient);
        GameManager.Instance.OnPerfectGumbo.AddListener(HandlePerfectGumbo);
    }

    void Update()
    {
        if (_state == FlowState.Wave1 && !_mireilleSpawned && GameManager.Instance.CountCoreIngredients() >= 3)
        {
            Spawn(mireillePrefab);
            _state = FlowState.Mireille;
            _mireilleSpawned = true;
        }
    }

    public void BeginTutorial()
    {
        _state = FlowState.Tutorial;
    }

    private void HandleCoreIngredient(string ing)
    {
        if (_state == FlowState.Tutorial && !_wave1Spawned && GameManager.Instance.CountCoreIngredients() >= 1)
        {
            SpawnWave(wave1Prefabs);
            _state = FlowState.Wave1;
            _wave1Spawned = true;
        }

        if (_state == FlowState.Mireille && GameManager.Instance.CountCoreIngredients() == 5)
        {
            // Player now eligible to trigger HenriDoor logic
            _state = FlowState.HenriDoor;
        }
    }

    private void HandlePerfectGumbo()
    {
        if (_state == FlowState.Mireille || _state == FlowState.HenriDoor)
        {
            Spawn(grandmaPrefab);
            _state = FlowState.Grandma;
        }
    }

    private void SpawnWave(IEnumerable<GameObject> list)
    {
        foreach (var p in list) Spawn(p);
    }

    private void Spawn(GameObject prefab)
    {
        if (!prefab) return;
        var t = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(prefab, t.position, Quaternion.identity);
    }
}
