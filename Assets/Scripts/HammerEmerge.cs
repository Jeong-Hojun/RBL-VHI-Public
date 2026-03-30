using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEmerge : MonoBehaviour
{
    private Animation anim;
    private Transform cat;
    private Transform dog;
    public bool _stateK;
    private int _staterep;
    // Start is called before the first frame update
    void Start()
    {
        cat = transform.GetChild(0);
        cat.gameObject.SetActive(false);

        dog = transform.GetChild(0);
        dog.gameObject.SetActive(false);

        anim = cat.GetComponent<Animation>();
        _stateK = false;
        _staterep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cat = transform.GetChild(0);
        if (Input.GetKeyDown(KeyCode.K) && _stateK == false)
        {
            cat.gameObject.SetActive(true);
            _stateK = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && _stateK == true)
        {
            cat.gameObject.SetActive(false);
            _stateK = false;
            _staterep = 0;
        }


        if (_stateK == true)
        {
            if (_staterep == 0)
            {
                try
                {
                    anim.Blend("AxeDrop");
                }
                catch
                {
                    anim.Blend("HammerDrop");
                }
                    

                
              
                _staterep += 1;
            }
            else
            {
                //anim.Blend("HammerDrop 1");
            }                                        
        }
    }

    public void OnClickLButton()
    {
        this.gameObject.transform.position = new Vector3(0, 0, (float)0.25);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    public void OnClickRButton()
    {
        this.gameObject.transform.position = new Vector3(0, 0, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}   
