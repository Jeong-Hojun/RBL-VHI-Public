using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;

public class FingerRotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 goal_angle;

    private float _taskstart;
    private float _taskend;
    private int _resttime;

    GameObject FR;


    Vector3 rotation;
    Vector3 tempvector_ext;
    Vector3 tempvector_flx;
    public bool isR = false;
    public bool isA = false;
    private float timer = 0;
   

    // Start is called before the first frame update
    void Start()
    {

        tempvector_ext.x = 0;
        tempvector_ext.y = 0;
        tempvector_ext.z = 0;
        tempvector_flx.x = 0;
        tempvector_flx.y = 0;
        tempvector_flx.z = 0;

        FR = transform.root.Find("HandModels").gameObject;
        _taskstart = FR.GetComponent<FingerAnimation>().taskstart;
        _taskend = FR.GetComponent<FingerAnimation>().taskend;
        _resttime = FR.GetComponent<FingerAnimation>().resttime;
    }

    // Update is called once per frame
    void Update()
    {

        rotation = this.transform.eulerAngles;

        if (isR == false && Input.GetKeyDown(KeyCode.R))
        {
            isR = true;
        }    
        else if (isR == true && Input.GetKeyDown(KeyCode.R))
        {
            isR = false;
            isA = false;
            tempvector_ext.x = 0;
            tempvector_ext.y = 0;
            tempvector_ext.z = 0;
            tempvector_flx.x = 0;
            tempvector_flx.y = 0;
            tempvector_flx.z = 0;
            timer = 0;
        }

        if(isR == true && isA == false && Input.GetKeyDown(KeyCode.A))
        {
            isA = true;
        }
        else if (isR == true && isA == true && Input.GetKeyDown(KeyCode.A))
        {
            isA = false;
        }


        if (isR == true && isA == true)
        {
            timer = timer + Time.deltaTime;

            if (timer >= _taskstart && timer < ((double)_taskstart + (double)_taskend)*1/2)
            {
                tempvector_ext = tempvector_ext + goal_angle * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(rotation - tempvector_ext);
            }
            else if (timer >= ((double)_taskstart + (double)_taskend)*1/2 && timer < _taskend)
            {
                tempvector_flx = tempvector_flx + goal_angle * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(rotation - tempvector_ext + tempvector_flx);
            }
            else if (timer >= _taskend && timer < _resttime)
            { 
                return;
            }
            else if (timer >= _resttime)
            {
                tempvector_ext.x = 0;
                tempvector_ext.y = 0;
                tempvector_ext.z = 0;
                tempvector_flx.x = 0;
                tempvector_flx.y = 0;
                tempvector_flx.z = 0;
                timer = 0;
            }
            else
            {
                return;
            }
            
        }
        else if (isR == true && isA == false)
        {
            tempvector_ext.x = 0;
            tempvector_ext.y = 0;
            tempvector_ext.z = 0;
            tempvector_flx.x = 0;
            tempvector_flx.y = 0;
            tempvector_flx.z = 0;
            timer = 0;
        }



    }

}
