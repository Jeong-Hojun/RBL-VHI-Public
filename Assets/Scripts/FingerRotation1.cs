using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerRotation1 : MonoBehaviour
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
    private bool isR = false;
    public bool isS = false;
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
        _taskstart = FR.GetComponent<FingerAnimationAO>().taskstart;
        _taskend = FR.GetComponent<FingerAnimationAO>().taskend;
        _resttime = FR.GetComponent<FingerAnimationAO>().resttime;
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
            isS = false;
            tempvector_ext.x = 0;
            tempvector_ext.y = 0;
            tempvector_ext.z = 0;
            tempvector_flx.x = 0;
            tempvector_flx.y = 0;
            tempvector_flx.z = 0;
            timer = 0;
        }

        if(isR == true && isS == false && Input.GetKeyDown(KeyCode.S))
        {
            isS = true;
        }
        else if (isR == true && isS == true && Input.GetKeyDown(KeyCode.S))
        {
            isS = false;
        }


        if (isR == true && isS == true)
        {
            timer = timer + Time.deltaTime;
            


            if (timer >= _taskstart && timer < ((double)_taskstart + (double)_taskend)*1/2)
            {
                tempvector_ext = tempvector_ext + goal_angle * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(rotation - tempvector_ext);
            }
            // else if (timer >= ((double)taskstart + (double)taskend) * 2 / 5 && timer < ((double)taskstart + (double)taskend) * 3/5)
            // {
            //      this.transform.rotation = Quaternion.Euler(rotation - tempvector_ext);
            // }
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
        
        
    }
}
