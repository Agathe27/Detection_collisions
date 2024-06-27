using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UXF;

public class PedestrianManager : MonoBehaviour
{
    public GameObject uiPanel;
    public float pedestrianSpawnInterval = 1.5f;
    public float moveTime = 3.5f;
    private float speed = 1.2f;
    public KeyPressHandler keyPressHandler;

    private Session session;
    private List<GameObject> pedestrians = new List<GameObject>();
    //dictionary = comme les listes mais les éléments n'ont pas d'ordre spécifique
    //they have two types and store data in pairs of those types called KeyValuePairs
    // le premier type = clé => chaque clé doit être unique 
    private Dictionary<string, List<Vector3>> destinationQueues = new Dictionary<string, List<Vector3>>();
    private int currentPedestrianIndex = 1;
    private GameObject currentPedestrian = null;
    private bool correct;

    void Start()
    {
        session = FindObjectOfType<Session>();
        session.experimentName = "Détection de collisions";

        if (session == null)
        {
            Debug.LogError("session non trouvée");
            return;
        }

        InitialisationDestinationsPietons();
        TrouverEtDesactiverPietons();

        // Commencer l'expérience
        StartCoroutine(StartExperiment());
    }

    // on initialise la file des destinations pour chaque piéton 
    private void InitialisationDestinationsPietons()
    {
        string[] tags = {
            "pieton_collision", "pieton_collision45","pieton_collision_45",
            "pieton_collision30", "pieton_collision_30", "pieton_collision20",
            "pieton_collision_20", "pieton_collision10", "pieton_collision_10"
        };

        foreach (string tag in tags)
        {
            List<Vector3> queue = new List<Vector3>();
            for (int i = 0; i < 5; i++)
            {
                queue.AddRange(GetDestinationsForTag(tag));
            }
            Shuffle(queue);
            destinationQueues[tag] = queue;
        }
    }

    //on commence la session en désactivant les piétons 
    private void TrouverEtDesactiverPietons()
    {
        string[] tags = {
            "pieton_collision", "pieton_collision45","pieton_collision_45",
            "pieton_collision30", "pieton_collision_30", "pieton_collision20",
            "pieton_collision_20", "pieton_collision10", "pieton_collision_10"
        };

        foreach (string tag in tags)
        {
            GameObject[] foundPedestrians = GameObject.FindGameObjectsWithTag(tag);
            pedestrians.AddRange(foundPedestrians);
        }

        // Désactiver tous les piétons au début
        foreach (var pedestrian in pedestrians)
        {
            pedestrian.SetActive(false);
        }
    }

    private IEnumerator StartExperiment()
    {
        int totalBlocks = 1; // voir pr le nb de blocs 
        int totalTrials = pedestrians.Count * 25; // 5 destinations par piéton, à répéter 5 fois chacun => 25 destinations

        for (int i = 0; i < totalBlocks; i++)
        {
            Block block = session.CreateBlock(totalTrials);
            yield return StartCoroutine(SpawnPedestrians(block));
        }

        session.End();
    }

    private List<Vector3> GetDestinationsForTag(string tag)
    {
        switch (tag)
        {
            case "pieton_collision":
                return new List<Vector3> {
                    new Vector3(9.05f, -2.2f, -0.5f),
                    new Vector3(9.05f, -2.2f, 0.5f),
                    new Vector3(9.05f, -2.2f, 2.5f),
                    new Vector3(9.05f, -2.2f, 3.5f),
                    new Vector3(9.05f, -2.2f, 1.5f)// emplacement de l'ACP
                };
            case "pieton_collision45":
                return new List<Vector3> {
                    new Vector3(7.05f, -2.2f, 1.5f),
                    new Vector3(8.05f, -2.2f, 1.5f),
                    new Vector3(9.05f, -2.2f, 1.5f),
                    new Vector3(10.05f, -2.2f, 1.5f),
                    new Vector3(11.05f, -2.2f, 1.5f)
                };
            case "pieton_collision_45":
                return new List<Vector3> {
                    new Vector3(7.05f, -2.2f, 1.5f),
                    new Vector3(8.05f, -2.2f, 1.5f),
                    new Vector3(9.05f, -2.2f, 1.5f),
                    new Vector3(10.05f, -2.2f, 1.5f),
                    new Vector3(11.05f, -2.2f, 1.5f)
                };
            case "pieton_collision30":
                return new List<Vector3> {
                    new Vector3(7.318f, -2.2f, 0.5f),
                    new Vector3(8.184f, -2.2f, 1f),
                    new Vector3(9.916f, -2.2f, 2f),
                    new Vector3(10.78f, -2.2f, 2.5f),
                    new Vector3(9.05f, -2.2f, 1.5f)
                };
            case "pieton_collision_30":
                return new List<Vector3> {
                    new Vector3(10.78f, -2.2f, 0.5f),
                    new Vector3(9.916f, -2.2f, 1f),
                    new Vector3(8.184f, -2.2f, 2f),
                    new Vector3(7.318f, -2.2f, 2.5f),
                    new Vector3(9.05f, -2.2f, 1.5f)
                };
            case "pieton_collision20":
                return new List<Vector3> {
                    new Vector3(7.764f, -2.2f, -0.03f),
                    new Vector3(8.407f, -2.2f, 0.734f),
                    new Vector3(9.69f, -2.2f, 2.266f),
                    new Vector3(10.336f, -2.2f, 3.032f),
                    new Vector3(9.05f, -2.2f, 1.5f)
                };
            case "pieton_collision_20":
                return new List<Vector3> {
                    new Vector3(10.336f, -2.2f, -0.03f),
                    new Vector3(9.69f, -2.2f, 0.734f),
                    new Vector3(8.407f, -2.2f, 2.266f),
                    new Vector3(7.764f, -2.2f, 3.032f),
                    new Vector3(9.05f, -2.2f, 1.5f)
                };
            case "pieton_collision10":
                return new List<Vector3> {
                    new Vector3(8.366f, -2.2f, -0.379f),
                    new Vector3(8.708f, -2.2f, 0.56f),
                    new Vector3(9.392f, -2.2f, 2.44f),
                    new Vector3(9.734f, -2.2f, 3.379f),
                    new Vector3(9.05f, -2.2f, 1.5f)
                };
            case "pieton_collision_10":
                return new List<Vector3> {
                    new Vector3(9.734f, -2.2f, -0.379f),
                    new Vector3(9.392f, -2.2f, 0.56f),
                    new Vector3(8.708f, -2.2f, 2.44f),
                    new Vector3(8.366f, -2.2f, 3.379f),
                    new Vector3(9.05f, -2.2f, 1.5f)
                };
            default:
                return new List<Vector3>();
        }
    }

    private Vector3 PositionInitiale(string tag)
    {
        switch (tag)
        {
            case "pieton_collision":
                return new Vector3(16.25f, -2.2f, 1.5f);
            case "pieton_collision45":
                return new Vector3(9.05f, -2.2f, -5.7f);
            case "pieton_collision_45":
                return new Vector3(9.05f, -2.2f, 8.7f);
            case "pieton_collision30":
                return new Vector3(12.65f, -2.2f, -4.9f);
            case "pieton_collision_30":
                return new Vector3(12.65f, -2.2f, 7.9f);
            case "pieton_collision20":
                return new Vector3(14.57f, -2.2f, -3.13f);
            case "pieton_collision_20":
                return new Vector3(14.57f, -2.2f, 6.13f);
            case "pieton_collision10":
                return new Vector3(15.82f, -2.2f, -0.96f);
            case "pieton_collision_10":
                return new Vector3(15.82f, -2.2f, 3.96f);
            default:
                return Vector3.zero;
        }
    }


    private IEnumerator SpawnPedestrians(Block block)
    {
        while (currentPedestrianIndex < block.trials.Count + 1)
        {
            Trial trial = block.trials[currentPedestrianIndex]; // début à index = 1 pour avoir les trials qui commencent à 1 => block.trials.Count +1 
            //Trial trial = block.GetRelativeTrial(currentPedestrianIndex);
            trial.Begin();

            Debug.Log("numéro de trial" + trial.number);

            GameObject pieton = pedestrians[currentPedestrianIndex % pedestrians.Count];
            Vector3 posInit = PositionInitiale(pieton.tag);
            pieton.transform.position = posInit;
            SpawnPedestrian(pieton);

            yield return new WaitForSeconds(pedestrianSpawnInterval + moveTime +1.5f);

            trial.End();
            currentPedestrianIndex++;
        }
        Debug.Log("Tous les piétons sont apparus 25 fois chacun");

    }

    public void SpawnNextPedestrian()
    {
        if (pedestrians.Count == 0)
        {
            Debug.LogWarning("No pedestrians to spawn.");
            return; // Ne rien faire s'il n'y a pas de piétons
        }

        GameObject pieton = pedestrians[currentPedestrianIndex % pedestrians.Count];
        Debug.Log("Spawning next pedestrian: " + pieton.name + ",index: " + currentPedestrianIndex);

        //keyPressHandler.EnregistrerData();

        // Obtenir la position initiale pour ce piéton
        string tag = pieton.tag;
        Vector3 initialPosition = PositionInitiale(tag);
        pieton.transform.position = initialPosition;

        SpawnPedestrian(pieton);
        currentPedestrianIndex++;
    }

    //initialisation des piétons avec unez destination + make sure que les destinations sont correctement distribuées et réutilisées de manière non consécutive
    /*private void SpawnPedestrian(GameObject pedestrian)
    {
        if (currentPedestrian != null)
        {
            Destroy(currentPedestrian);
            HideUIPanel();
        }

        pedestrian.SetActive(true);

        string tag = pedestrian.tag;
        if (destinationQueues.ContainsKey(tag))
        {
            List<Vector3> destinations = destinationQueues[tag];
            Vector3 destination = destinations[0];
            destinations.RemoveAt(0);
            destinations.Add(destination); //pour être sûr que la destination est réutilisée plus tard 
            if (pedestrian.GetComponent<MoveStraight>() != null)
            {
                MoveStraight moveStraight = pedestrian.GetComponent<MoveStraight>();
                moveStraight.Initialize(uiPanel, moveTime, speed, destination, OnPedestrianStopped);
            }
        }

        currentPedestrian = pedestrian;
    }*/

    //méthode pour activer le piéton mis en paramètre ; s'assurer qu'il n'y a pas de piétons actifs avant
    private void SpawnPedestrian(GameObject pedestrian)
    {
        if (currentPedestrian != null)
        {
            currentPedestrian.SetActive(false);
            HideUIPanel();
        }

        pedestrian.SetActive(true);

        string tag = pedestrian.tag;
        if (destinationQueues.ContainsKey(tag))
        {
            List<Vector3> destinations = destinationQueues[tag];
            if (destinations.Count == 0)
            {
                Debug.LogWarning("No more destinations available for tag: " + tag);
                return;
            }

            Vector3 destination = destinations[0];
            destinations.RemoveAt(0);
            destinations.Add(destination); // Réutiliser la destination plus tard

            if (pedestrian.GetComponent<MoveStraight>() != null)
            {
                MoveStraight moveStraight = pedestrian.GetComponent<MoveStraight>();
                //moveStraight.Initialize(uiPanel, moveTime, speed, destination, OnPedestrianStopped, keyPressHandler);
                moveStraight.Initialize(uiPanel, moveTime, speed, destination, OnPedestrianStopped);

                Debug.Log("Piéton activé, destination : " + destination);

                correct = (destination.x == 9.05f && destination.z == 1.5f);
                Debug.Log(correct);
                session.CurrentTrial.result["collision"] = correct;
                // session.GetTrial().Begin();
            }
        }

        currentPedestrian = pedestrian;
    }

    private void OnPedestrianStopped(GameObject pedestrian)
    {
        if (currentPedestrian == pedestrian)
        {
            Destroy(currentPedestrian);
            currentPedestrian = null;
            //session.GetTrial().End();
        }
    }

    private void HideUIPanel()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    //mélanger les destinations pour s'assurer que l'ordre est aléatoire 
    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}