using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    private Animator animator;
    private bool IsAnimating = false;
    private bool IsReady = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsAnimating", false);
        animator.SetBool("IsReady", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReady == false)
        {
            animator.GetComponent<Animator>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && IsReady == false)
        {
            animator.GetComponent<Animator>().enabled = true;
            animator.Play("Idle",-1,0f);
            IsReady = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.R) && IsReady == true)
        {
            IsReady = false;
        }

        if (Input.GetKeyDown(KeyCode.A) && IsAnimating == false && IsReady == true)
        {
            animator.SetBool("IsAnimating", true);
            IsAnimating = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && IsAnimating == true && IsReady == true)
        {
            animator.SetBool("IsAnimating", false);
            IsAnimating = false;
        }
        
    }
}
