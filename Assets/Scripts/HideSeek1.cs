using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSeek1 : MonoBehaviour
{
    private Transform cylinder;


    public bool _bool;
    //public int count;

    // Start is called before the first frame update
    void Start()
    {
        cylinder = this.gameObject.transform.Find("Cylinder");
        

        _bool = false;
        cylinder.gameObject.SetActive(false);
        
        //count = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        cylinder = this.gameObject.transform.Find("Cylinder");
        
        if (_bool == false)
        {
            cylinder.gameObject.SetActive(false);
            
        }
        else
        {
            cylinder.gameObject.SetActive(true);

        }

        //cylinder1 = this.gameObject.transform.Find("Cylinder01");
        //cylinder2 = this.gameObject.transform.Find("Cylinder02");
        //cylinder3 = this.gameObject.transform.Find("Cylinder03");


        //if (Input.GetKeyDown(KeyCode.H) && _bool == false )
        //{
        //    _bool = true;
        //    cylinder1.gameObject.SetActive(true);
        //    cylinder2.gameObject.SetActive(true);
        //    cylinder3.gameObject.SetActive(true);

        //}
        //else if (Input.GetKeyDown(KeyCode.H) && _bool == true)
        //{
        //    cylinder1.gameObject.SetActive(false);
        //    cylinder2.gameObject.SetActive(false);
        //    cylinder3.gameObject.SetActive(false);
        //    _bool = false;
        //}

    }

    public void OnClickVTButton()
    {
        _bool = true;
        //count += 1;
    }


    public void OnClickAOButton()
    {
        _bool = false;
        //count += 1;
    }
}
