using UnityEngine;
using UnityEngine.UI;
using UXF;

public class BeginSessionButton : MonoBehaviour
{
    public Button beginButton;
    public GameObject pedestrianManager;
    public GameObject keyPressHandler;
    public GameObject cameraMovement;

    void Start()
    {
        if (beginButton != null)
        {
            beginButton.onClick.AddListener(OnBeginSession);
        }

        // Désactiver les scripts au démarrage
        if (pedestrianManager != null)
            pedestrianManager.SetActive(false);
        if (keyPressHandler != null)
            keyPressHandler.SetActive(false);
        if (cameraMovement != null)
            cameraMovement.SetActive(false);
    }

    void OnBeginSession()
    {
        // Activer tous les objets nécessaires
        if (pedestrianManager != null)
            pedestrianManager.SetActive(true);
        if (keyPressHandler != null)
            keyPressHandler.SetActive(true);
        if (cameraMovement != null)
            cameraMovement.SetActive(true);

        Debug.LogError("Aucune session trouvée !");
        }
   }