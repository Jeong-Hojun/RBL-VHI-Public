using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeTime1 : MonoBehaviour
{
    [SerializeField]
    private int fingerNum;
    [SerializeField]
    private int taskstart;
    [SerializeField]
    private int taskend;
    [SerializeField]
    private int restend;

    public Material[] materials;
    public Renderer rend;

    private int index;
    private int space_indicator;
    private float timer;

    private void Start()
    {
        index = 1;
        space_indicator = 0;
        timer = 0;
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


            if (timer > 0 && timer < taskstart)
            {
                index = 2;
                rend.sharedMaterial = materials[index - 1];

            }
            else if (timer >= taskstart && timer < taskend)
            {
                index = 1;
                rend.sharedMaterial = materials[index - 1];
            }
            else if (timer >= taskend && timer < restend)
            {
                index = 2;
                rend.sharedMaterial = materials[index - 1];
                
            }
            else
            {
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