using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationMotorTrigger : MonoBehaviour
{

    private int space_indicator;
    private float timer;
    private int tasktime;
    private int resttime;
    public bool motorvib;

    // Start is called before the first frame update
    void Start()
    {
        space_indicator = 0;
        timer = 0;
        tasktime = 1;
        resttime = 1;
        motorvib = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && space_indicator == 0)
        {
            space_indicator = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && space_indicator == 1)
        {
            space_indicator = 0;
            return;
        }

        if (space_indicator == 1)
        {
            timer += Time.deltaTime;

            motorvib = true;

            if (timer >= tasktime && timer < tasktime + resttime)
            {
                motorvib = false;

            }
            else if (timer >= tasktime + resttime)
            {
                motorvib = true;
                timer = 0;
            }
        }

        if (space_indicator == 0)
        {
            timer = 0;
            motorvib = false;

        }

    }
}
