using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitChangeColor : MonoBehaviour
{
    public bool _hitIndicator;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        _hitIndicator = gameObject.GetComponentInChildren<HitDetection>().hitIndicator;
        StartCoroutine(OnHit());
    }
    
    IEnumerator OnHit()
    {
        if (_hitIndicator == true)
        {
            mat.color = Color.red;
            yield return new WaitForSeconds(1f);
            mat.color = Color.white;
        }
       
    }
}

