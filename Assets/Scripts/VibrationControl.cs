using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationControl : MonoBehaviour
{
    private float timer;
    private int tasktime;
    private int resttime;
    private bool shaking;
    private Quaternion originRotation;
    [SerializeField]
    private float shake_decay;
    [SerializeField]
    private float shake_intensity;

    private void Start()
    {
        timer = 0;
        tasktime = 1;
        resttime = 2;
        shaking = false;
        Vector3 originalPos = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && shaking == false)
        {
            shaking = true;
            timer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && shaking == true)
        {
            shaking = false;
        }

        if (shaking == true)
        {
            timer += Time.deltaTime;

            originRotation = transform.rotation;
            
            if (shake_intensity > 0 && timer >= tasktime && timer < tasktime + resttime)
                {
                transform.rotation = new Quaternion(
                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
                shake_intensity -= shake_decay;
                }
            else if (shake_intensity > 0 && timer >= tasktime + resttime)
            {
                timer = 0;
            }

        }
        
    }

}
