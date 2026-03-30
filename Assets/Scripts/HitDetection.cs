using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public bool hitIndicator;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        hitIndicator = false;
        mat = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(OnHit());

    }
    
    IEnumerator OnHit()
    {
        hitIndicator = true;
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        
        hitIndicator = false;
        mat.color = Color.white;
    }
}

