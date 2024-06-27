using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    //private float speed = ToggleManager.Instance.cameraSpeed; // Vitesse de la caméra
    public float speed = 1.2f;
    public float moveDuration = 3.5f; // Durée de déplacement en secondes
    public float stopDuration = 1.5f; // Durée de l'arrêt en secondes

    private Vector3 initialPosition;
    private PedestrianManager pedestrianManager;


    void Start()
    {
        initialPosition = new Vector3(transform.position.x, -0.6f, transform.position.z);
        transform.position = initialPosition;
        pedestrianManager = FindObjectOfType<PedestrianManager>();
        StartCoroutine(MoveCameraRepeatedly());
    }

    private IEnumerator MoveCameraRepeatedly()
    {
        while (true)
        {
            float elapsedTime = 0f;

            // Mouvement de la caméra vers l'avant
            while (elapsedTime < moveDuration)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, -0.6f, transform.position.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Arrêt de la caméra pendant stopDuration secondes
            yield return new WaitForSeconds(stopDuration);

            // Remettre la caméra à sa position initiale
            transform.position = initialPosition;

            // Informer le PedestrianManager qu'un nouveau piéton peut apparaître
            pedestrianManager.SpawnNextPedestrian();
        }
    }
}
/*using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float moveDuration = 3.5f; // Durée en secondes
    public float stopDuration = 1.5f; // Durée de l'arrêt en secondes

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = new Vector3(transform.position.x, -0.6f, transform.position.z);
        transform.position = initialPosition;
        StartCoroutine(MoveCameraRepeatedly());
    }

    private IEnumerator MoveCameraRepeatedly()
    {
        while (true)
        {
            float elapsedTime = 0f;
            float speed = ToggleManager.Instance.cameraSpeed;

            // Mouvement de la caméra vers l'avant
            while (elapsedTime < moveDuration)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, -0.6f, transform.position.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Arrêt de la caméra pendant stopDuration secondes
            yield return new WaitForSeconds(stopDuration);

            // Remettre la caméra à sa position initiale
            transform.position = initialPosition;

            // Attendre stopDuration secondes avant de recommencer le cycle
            yield return new WaitForSeconds(stopDuration);
        }
    }
}*/