using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour
{
    public Material[] materials;
    public Renderer rend;

    private int index = 1;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;

    }

    private void OnMouseDown()
    {
        if (materials.Length == 0)
        {
            return;
        }    
        if (Input.GetMouseButtonDown(0))
        {
            index += 1;
            if (index == materials.Length + 1)
            {
                index = 1;
            }
            print(index);

            rend.sharedMaterial = materials[index - 1];
        }
    }
}