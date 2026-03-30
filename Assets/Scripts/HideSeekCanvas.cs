using UnityEngine;

/// <summary>
/// Manages canvas visibility for the therapy control panel.
/// When _bool is true (ON state), all child elements are visible and Button_Back is hidden.
/// When _bool is false (OFF state), child elements are hidden and Button_Back is shown.
/// </summary>
public class HideSeekCanvas : MonoBehaviour
{
    private Transform canvas;
    public bool _bool;

    void Start()
    {
        canvas = this.gameObject.transform;
        _bool = true;
        SetCanvasState(true);
    }

    void Update()
    {
        SetCanvasState(_bool);
    }

    /// <summary>
    /// Shows/hides canvas children and toggles the back button accordingly.
    /// </summary>
    private void SetCanvasState(bool isOn)
    {
        foreach (Transform child in canvas.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(isOn || child.name == transform.name);
        }

        Transform backButton = canvas.Find("Button_Back");
        backButton.gameObject.SetActive(!isOn);
        foreach (Transform child in backButton.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(!isOn);
        }
    }

    public void OnClickONButton() => _bool = true;
    public void OnClickOFFButton() => _bool = false;
}
