using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeTime : MonoBehaviour
{
    public Material[] materials;
    public Renderer rend;

    private int index;
    private int space_indicator;
    private float timer;
    private int tasktime;
    private int resttime;

    private void Start()
    {
        index = 1;
        space_indicator = 0;
        timer = 0;
        tasktime = 1;
        resttime = 2;
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        

    }

    private void Update()
    {
   

        if (materials.Length == 0)
        {
            return;
        }
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

            index = 2;

            print(index);

            rend.sharedMaterial = materials[index - 1];

            if (timer >= tasktime && timer < tasktime + resttime)
            {
                index = 1;
                rend.sharedMaterial = materials[index - 1];
            }
            else if (timer >= tasktime + resttime)
            {
                index = 2;
                rend.sharedMaterial = materials[index - 1];
                timer = 0;
            }
        }
           
        if (space_indicator == 0)
        {
            timer = 0;
            index = 2;
            rend.sharedMaterial = materials[index - 1];
        }

     


    }
}