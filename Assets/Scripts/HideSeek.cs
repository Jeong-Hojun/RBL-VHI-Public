using UnityEngine;

/// <summary>
/// Shows or hides the three reaching task cylinder targets.
/// Controlled by UI buttons: VM button shows them, AO button hides them.
/// </summary>
public class HideSeek : MonoBehaviour
{
    private Transform cylinder1;
    private Transform cylinder2;
    private Transform cylinder3;

    public bool _bool;

    void Start()
    {
        cylinder1 = this.gameObject.transform.Find("Cylinder01");
        cylinder2 = this.gameObject.transform.Find("Cylinder02");
        cylinder3 = this.gameObject.transform.Find("Cylinder03");

        _bool = false;
        cylinder1.gameObject.SetActive(false);
        cylinder2.gameObject.SetActive(false);
        cylinder3.gameObject.SetActive(false);
    }

    void Update()
    {
        bool active = _bool;
        cylinder1.gameObject.SetActive(active);
        cylinder2.gameObject.SetActive(active);
        cylinder3.gameObject.SetActive(active);
    }

    public void OnClickVMButton() => _bool = true;
    public void OnClickAOButton() => _bool = false;
}
