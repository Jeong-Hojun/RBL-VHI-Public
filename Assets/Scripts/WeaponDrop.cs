using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{

    public Vector3 posOffset;
    public Vector3 initOri;
    private Vector3 initPos;
    private Vector3 finPos;
    private Vector3 finOri;
    private Vector3 handPos;
    private Vector3 curPos;


    private Transform Knife;
    private Transform HandCon;
    private float v;

    public bool _stateStop;
    public bool _state;
    public bool _stateK;
    public int _repetition;

    // Start is called before the first frame update
    void Start()
    {
        initOri = new Vector3(180f, 0f, 0f);
        finPos = new Vector3(0f, 0f, 0f);
        finOri = new Vector3(0f, 0f, 0f);
        posOffset = new Vector3(0.09f, -0.2f, 0.07f);
        _repetition = 0;

        HandCon = GameObject.Find("HandModels").transform.Find("Hand_04_R").transform.Find("HandContainer").transform.Find("R_Wrist");
        handPos = HandCon.transform.position;


        //Knife = GameObject.Find("GSR").transform.Find("Steel_Dagger_4096");
        Knife = GameObject.Find("HandModels").transform.Find("Hand_04_R").transform.Find("HandContainer").transform.Find("Steel_Dagger_4096");
        Knife.gameObject.SetActive(false);
        _state = false;
        _stateStop = Knife.gameObject.GetComponent<Collision_Detection>()._stateStop;

        initPos = handPos + posOffset;      

        Knife.transform.localPosition = new Vector3(0f,0f,0f) + posOffset;
        Knife.transform.position = Knife.transform.position + new Vector3(0, 0.2f, 0);
        Knife.transform.rotation = Quaternion.Euler(initOri);

        //Knife.transform.localPosition = initPos;
        //Knife.transform.localRotation = Quaternion.Euler(initOri);
    }

    // Update is called once per frame
    void Update()
    {
        Knife = GameObject.Find("HandModels").transform.Find("Hand_04_R").transform.Find("HandContainer").transform.Find("Steel_Dagger_4096");
        _stateStop = Knife.gameObject.GetComponent<Collision_Detection>()._stateStop;
        //Knife = GameObject.Find("GSR").transform.Find("Steel_Dagger_4096");

        if (Input.GetKeyDown(KeyCode.K) && _state == false)
        {
            _state = true;
            //GameObject.Find("GSR").transform.Find("Steel_Dagger_4096").gameObject.SetActive(true);
            Knife.gameObject.SetActive(true);
            _stateK = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && _state == true)
        {
            _state = false;
            _stateK = false;
            Knife.gameObject.SetActive(false);

        }

        if (_stateK == true)
        {
            MovingKnife();
        }
        else
        {
            
            initPos = handPos + posOffset;

            Knife.transform.localPosition = new Vector3(0f, 0f, 0f) + posOffset;
            Knife.transform.position = Knife.transform.position + new Vector3(0, 0.2f, 0);
            Knife.transform.rotation = Quaternion.Euler(initOri);
        }

        //if(_state == true)
        //{
        //    GameObject.Find("GSR").transform.Find("Steel_Dagger_4096").gameObject.SetActive(true);
        //}
        //else if (_state == false)q
        //{
        //    Knife.gameObject.SetActive(false);
        //}

    }

    private void MovingKnife()
    {
        Vector3 temp;
        curPos = Knife.transform.position;
        temp = curPos;

        if (_stateStop == false)
        {
            v += 0.001f;
            temp.y -= v;
            
            Knife.transform.position = temp;
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        _stateStop = true;
    }
}
