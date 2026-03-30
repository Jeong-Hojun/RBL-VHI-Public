using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Manages the reaching task sequence for three cylinder targets (cy1, cy2, cy3).
/// Generates a random non-repeating target order and advances through it on a timer.
///
/// V key starts/stops the task (Virtual Motor mode).
/// presentSequence (1–3) indicates the currently active target cylinder.
/// isVM is true while a target is being displayed, used by ArduinoControl for stimulation.
/// </summary>
public class ReachingTaskParent1 : MonoBehaviour
{
    GameObject cy1;
    GameObject cy2;
    GameObject cy3;

    public int[] targetArray;

    [SerializeField] public int setRepeatNumber; // Number of targets per session
    [SerializeField] public int setTimeMax;      // Max session duration (seconds)
    [SerializeField] public int targetTime;      // Time per target (seconds)

    public int pastSequence;
    public int presentSequence;      // Current target cylinder index (1–3)
    public int presentRepeatNumber;  // Current position in targetArray

    private bool _hitIndicator1 = false;
    private bool _hitIndicator2 = false;
    private bool _hitIndicator3 = false;

    private bool startIndicator = false;
    private bool temp = false;

    private float timer;
    public float timer_rep;
    public bool isVM; // True while target is highlighted (used for Arduino stimulation)

    void Start()
    {
        cy1 = transform.GetChild(0).gameObject;
        cy2 = transform.GetChild(1).gameObject;
        cy3 = transform.GetChild(2).gameObject;

        presentRepeatNumber = 0;
        targetArray = GetRandomArray(setRepeatNumber, 1, 4);
        targetArray[setRepeatNumber - 1] = 1; // Always end on target 1
        timer = 0;
        timer_rep = 0;
        isVM = false;
    }

    void Update()
    {
        _hitIndicator1 = cy1.GetComponent<ReachingTask>()._hitIndicator;
        _hitIndicator2 = cy2.GetComponent<ReachingTask>()._hitIndicator;
        _hitIndicator3 = cy3.GetComponent<ReachingTask>()._hitIndicator;

        // V key toggles the reaching task session
        if (!startIndicator && Input.GetKeyDown(KeyCode.V))
        {
            startIndicator = true;
            presentSequence = targetArray[0];
            presentRepeatNumber = 0;
            isVM = true;
        }
        else if (startIndicator && Input.GetKeyDown(KeyCode.V))
        {
            startIndicator = false;
            isVM = false;
        }

        if (startIndicator)
        {
            timer += Time.deltaTime;
            timer_rep += Time.deltaTime;

            // Hide target highlight after 1 second (inter-stimulus interval)
            if (timer_rep >= 1)
                isVM = false;

            // Advance to next target after targetTime seconds
            if (timer_rep >= targetTime)
            {
                pastSequence = presentSequence;
                timer_rep = 0;
                StartCoroutine(NextTarget(targetArray[presentRepeatNumber]));
                isVM = true;
            }

            if (timer > setTimeMax)
                startIndicator = false;
        }

        if (!startIndicator)
            presentSequence = 0;

        // Track repeat number changes to count completed targets
        if (presentSequence != pastSequence)
        {
            if (!temp)
            {
                presentRepeatNumber += 1;
                temp = true;
            }
        }
        else
        {
            temp = false;
        }

        if (presentRepeatNumber + 1 > setRepeatNumber)
            startIndicator = false;
    }

    /// <summary>
    /// Waits 1 second then updates the active target cylinder.
    /// </summary>
    IEnumerator NextTarget(int targetNumber)
    {
        yield return new WaitForSeconds(1f);
        presentSequence = targetNumber;
    }

    /// <summary>
    /// Generates an array of random integers in [min, max) with no consecutive duplicates.
    /// </summary>
    public int[] GetRandomArray(int length, int min, int max)
    {
        int[] randArray = new int[length];
        randArray[0] = UnityEngine.Random.Range(min, max);

        for (int i = 0; i < length - 1; ++i)
        {
            do
            {
                randArray[i + 1] = UnityEngine.Random.Range(min, max);
            }
            while (randArray[i] == randArray[i + 1]);
        }

        return randArray;
    }
}
