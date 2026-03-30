using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Detection : MonoBehaviour
{

    public bool _stateStop;
    public bool stateK;
    private Transform HandModel;


    // Start is called before the first frame update
    void Start()
    {
        _stateStop = false;
        HandModel = GameObject.Find("HandModels").transform.Find("Hand_04_R").transform.Find("HandContainer").transform.Find("R_Wrist");
    }

    // Update is called once per frame
    void Update()
    {
        stateK = HandModel.gameObject.GetComponent<WeaponDrop>()._stateK;

        if(stateK == false)
        {
            _stateStop = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _stateStop = true;
    }
}
