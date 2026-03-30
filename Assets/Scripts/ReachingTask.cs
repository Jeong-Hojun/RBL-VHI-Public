using System.Collections;
using UnityEngine;

/// <summary>
/// Attached to each cylinder target in the reaching task.
/// Changes color to yellow when this cylinder is the active target,
/// then to blue briefly when the hand makes contact, then back to white.
///
/// taskSequence must match the index used in ReachingTaskParent1 (1, 2, or 3).
/// </summary>
public class ReachingTask : MonoBehaviour
{
    public bool _hitIndicator;          // True when hand is touching this target
    private bool startIndicator = false;
    public bool sequenceIndicator = false; // True when this cylinder is the active target
    public int _presentSequence;
    public int _presentRepeatNumber;
    private int pastRepatNumber;

    [SerializeField] private int taskSequence; // This cylinder's sequence index (1–3)

    Material mat;
    GameObject RT; // Reference to ReachingTaskParent1

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        RT = transform.parent.gameObject;
    }

    void Update()
    {
        _presentSequence = RT.GetComponent<ReachingTaskParent1>().presentSequence;
        _presentRepeatNumber = RT.GetComponent<ReachingTaskParent1>().presentRepeatNumber;

        // V key enables hit detection for this session
        if (Input.GetKeyDown(KeyCode.V) && !startIndicator)
            startIndicator = true;

        // Highlight this cylinder when it becomes the active target
        if (taskSequence == _presentSequence && startIndicator && pastRepatNumber != _presentRepeatNumber)
        {
            sequenceIndicator = true;
            mat.color = Color.yellow;
            startIndicator = false;
        }

        pastRepatNumber = _presentRepeatNumber;
        _hitIndicator = gameObject.GetComponentInChildren<HitDetection>().hitIndicator;

        StartCoroutine(OnHit());
    }

    /// <summary>
    /// Flashes the cylinder blue when a valid hit is detected, then resets to white.
    /// </summary>
    IEnumerator OnHit()
    {
        if (_hitIndicator && sequenceIndicator)
        {
            mat.color = Color.blue;
            yield return new WaitForSeconds(1f);
            sequenceIndicator = false;
            mat.color = Color.white;
            startIndicator = true;
        }
    }
}
