/*using UnityEngine;

public class ToggleManager : MonoBehaviour
{
    public static ToggleManager Instance { get; private set; }

    public float pedestrianSpeed = 1.2f; // Vitesse par défaut
    public float cameraSpeed = 1.2f; // Vitesse par défaut

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        pedestrianSpeed = speed;
        cameraSpeed = speed;
    }
}*/