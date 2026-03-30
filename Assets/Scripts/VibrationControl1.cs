using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationControl1 : MonoBehaviour
{

    [SerializeField]
    private int taskstart;
    [SerializeField]
    private int taskend;
    [SerializeField]
    private int restend;

    private float timer;
    private bool shaking;
    private Quaternion originRotation;
    [SerializeField]
    private float shake_growth;
    [SerializeField]
    private float shake_decay;
    [SerializeField]
    private float shake_intensity_set;
    private float shake_intensity;

    private void Start()
    {
        timer = 0;
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
            
            if (shake_intensity_set > 0 &&  timer > 0 && timer < taskstart)
            {
                return;
            }
            else if (shake_intensity_set > 0 && shake_intensity <= shake_intensity_set && timer >= taskstart && timer < taskend)
            {
                shake_intensity += shake_growth;
                transform.rotation = new Quaternion(
                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
            }
            else if (shake_intensity_set > 0 && timer >= taskstart && timer < taskend)
                {
                shake_intensity = shake_intensity_set;
                transform.rotation = new Quaternion(
                    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
                    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
                shake_intensity -= shake_decay;
                }
            else if (shake_intensity_set > 0 && timer >= taskend && timer < restend)
            {
                return;
            }
            else
            {
                timer = 0;
            }
        }
        
    }

}
