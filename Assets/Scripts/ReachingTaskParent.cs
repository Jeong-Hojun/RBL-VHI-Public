using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;


public class ReachingTaskParent : MonoBehaviour
{
    GameObject cy1;
    GameObject cy2;
    GameObject cy3;
    public int[] targetArray;

    [SerializeField]
    public int setRepeatNumber;
    [SerializeField]
    public int setTimeMax;

    private int pastSequence;
    public int presentSequence;
    public int presentRepeatNumber;
    

    private bool _hitIndicator1 = false;
    private bool _hitIndicator2 = false;
    private bool _hitIndicator3 = false;

    private bool startIndicator = false;

    private bool temp = false;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        cy1 = transform.GetChild(0).gameObject;
        cy2 = transform.GetChild(1).gameObject;
        cy3 = transform.GetChild(2).gameObject;
        presentRepeatNumber = 0;
        targetArray = GetRandomArray(setRepeatNumber, 1, 4);
        targetArray[setRepeatNumber - 1] = 1;
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
        _hitIndicator1 = cy1.GetComponent<ReachingTask>()._hitIndicator;
        _hitIndicator2 = cy2.GetComponent<ReachingTask>()._hitIndicator;
        _hitIndicator3 = cy3.GetComponent<ReachingTask>()._hitIndicator;

        if (startIndicator == false && Input.GetKeyDown(KeyCode.V))
        {
            startIndicator = true;
            presentSequence = targetArray[0];
            presentRepeatNumber = 0;

        }
        else if (startIndicator == true && Input.GetKeyDown(KeyCode.V))
        {
            startIndicator = false;
        }

        if (startIndicator == true )
        {
            timer += Time.deltaTime;

            if (_hitIndicator1 == true && presentSequence == 1)
            {
                pastSequence = 1;
                StartCoroutine(NextTarget(targetArray[presentRepeatNumber]));
                
            }
            else if (_hitIndicator2 == true && presentSequence == 2)
            {
                pastSequence = 2;
                StartCoroutine(NextTarget(targetArray[presentRepeatNumber]));

            }
            else if (_hitIndicator3 == true && presentSequence == 3)
            {
                pastSequence = 3;
                StartCoroutine(NextTarget(targetArray[presentRepeatNumber]));

            }

            if (timer > setTimeMax)
            {
                startIndicator = false;
            }
        }
        if (startIndicator == false)
        {
            presentSequence = 0;
        }

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
        {
            startIndicator = false;
        }

    }
    IEnumerator NextTarget(int targetNumber)
    {
        yield return new WaitForSeconds(1f);
        yield return presentSequence = targetNumber;

    }

    public int[] GetRandomArray(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;
        randArray[0] = UnityEngine.Random.Range(min, max);

        for (int i=0; i<length-1; ++i)
        {
            while (true)
            {
                randArray[i+1] = UnityEngine.Random.Range(min, max);
                isSame = false;
                if (randArray[i] == randArray[i + 1])
                {
                    isSame = true;
                }

                if (!isSame) break;

            }

        }

        return randArray;

    }
       
}
