/*using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Toggle slowToggle;
    public Toggle fastToggle;

    private void Start()
    {
        slowToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        fastToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
    }

    private void OnToggleChanged()
    {
        if (slowToggle.isOn)
        {
            ToggleManager.Instance.SetSpeed(0.8f);
        }
        else if (fastToggle.isOn)
        {
            ToggleManager.Instance.SetSpeed(1.2f);
        }
    }
}*/