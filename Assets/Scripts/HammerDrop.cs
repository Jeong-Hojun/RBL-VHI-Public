using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDrop : MonoBehaviour
{
    private Animation anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.Blend("HammerDrop");
        }
    }
}
