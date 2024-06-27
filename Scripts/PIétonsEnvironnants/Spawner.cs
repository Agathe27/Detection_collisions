using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject[] listePietons;
    private int nbPieton;
    public Transform[] waypoints; // Ajoutez un tableau de waypoints
    public float spawnRadius = 1.0f; // Rayon autour du point de spawn pour décaler les piétons

    void Start()
    {
        nbPieton = Random.Range(0, 3); 
        for (int i = 0; i < nbPieton; i++)
        {
            SpawnPieton();
        }
        Invoke("SpawnPieton", 3); // utilise la méthode toutes les 3 secondes
    }

    void SpawnPieton()
    {
        int nombreAléatoire = Random.Range(0, listePietons.Length);

        // Calculer une position de spawn légèrement décalée
        Vector3 spawnPosition = this.transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = this.transform.position.y; // Maintenir la hauteur

        GameObject pieton = Instantiate(listePietons[nombreAléatoire], spawnPosition, Quaternion.identity);
        NavMeshAgent agent = pieton.GetComponent<NavMeshAgent>();
        if (agent != null && waypoints.Length > 0)
        {
            if (agent.isOnNavMesh)
            {
                Vector3 destination = waypoints[0].position;
                Debug.Log("Setting destination to: " + destination);
                agent.SetDestination(destination); // Déplace le piéton vers le premier waypoint
            }
            else
            {
                Debug.LogError("NavMeshAgent is not on a NavMesh");
            }
        }
        Invoke("SpawnPieton", Random.Range(5, 10));
    }
}