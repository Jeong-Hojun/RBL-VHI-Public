using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSeek2 : MonoBehaviour
{
    private Transform leftHand;
    private Transform rightHand;


    public bool _bool;
    //public int count;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = this.gameObject.transform.Find("Hand_04_L");
        rightHand = this.gameObject.transform.Find("Hand_04_R");


        _bool = false;

        // obj is your GameObject change it with your

        leftHand.gameObject.layer = LayerMask.NameToLayer("HitBox");
        foreach (Transform child in leftHand.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("HitBox");  // add any layer you want. 
        }
        
        rightHand.gameObject.layer = LayerMask.NameToLayer("HandModel");
        foreach (Transform child in rightHand.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("HandModel");  // add any layer you want. 
        }

        //count = 0;

    }

    // Update is called once per frame
    void Update()
    {
        leftHand = this.gameObject.transform.Find("Hand_04_L");
        rightHand = this.gameObject.transform.Find("Hand_04_R");

        if (_bool == false)
        {
            leftHand.gameObject.layer = LayerMask.NameToLayer("HitBox");
            foreach (Transform child in leftHand.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("HitBox");  // add any layer you want. 
            }

            rightHand.gameObject.layer = LayerMask.NameToLayer("HandModel");
            foreach (Transform child in rightHand.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("HandModel");  // add any layer you want. 
            }

        }
        else
        {
            leftHand.gameObject.layer = LayerMask.NameToLayer("HandModel");
            foreach (Transform child in leftHand.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("HandModel");  // add any layer you want. 
            }

            rightHand.gameObject.layer = LayerMask.NameToLayer("HitBox");
            foreach (Transform child in rightHand.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("HitBox");  // add any layer you want. 
            }

        }



    }

    public void OnClickLHButton()
    {
        _bool = true;
        //count += 1;
    }


    public void OnClickRHButton()
    {
        _bool = false;
        //count += 1;
    }
}
