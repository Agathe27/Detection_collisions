using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MarcheAutre : MonoBehaviour
{
    public GameObject[] waypoints;
    private GameObject waypointFinal; // waypoint spé où les piétons disparaissent
    int currentWP = 0;
    bool allerVersWpFinal = false;

    public float speed = 1.2f;
    //private float speed = ToggleManager.Instance.pedestrianSpeed;
    public float rotationSpeed = 2; // Vitesse de rotation
    public int minRandomWaypoints = 1; // Nombre minimal de waypoints aléatoires avant de se diriger vers le waypoint final
    public int maxRandomWaypoints = 3; // Nombre maximal de waypoints aléatoires avant de se diriger vers le waypoint final
    public float rayonEvitement = 0.7f;
    private int randomWaypointCount = 0; // Compteur de waypoints aléatoires atteints

    public float timeInLine = 5.0f; // Temps d'attente avant de marcher
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = speed;
        agent.angularSpeed = rotationSpeed * 100;
        anim.speed = speed;

        randomWaypointCount = Random.Range(minRandomWaypoints, maxRandomWaypoints + 1);

        anim.SetBool("isWalking", false);
        agent.isStopped = true; // le piéton est dans la queue
        StartCoroutine(WaitAndStartWalking());
    }

    IEnumerator WaitAndStartWalking()
    {
        // Attendre pendant le temps spécifié avant de commencer à marcher
        yield return new WaitForSeconds(timeInLine);

        // Commencer à marcher après l'attente
        anim.SetBool("isWalking", true);
        agent.isStopped = false; 
    }

    void Update()
    {
        //Debug.Log("speed " + speed);

        if (anim.GetBool("isWalking"))
        {
          if (allerVersWpFinal)
            {
                agent.SetDestination(waypointFinal.transform.position);
                if (Vector3.Distance(transform.position, waypointFinal.transform.position) < 1)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                FollowRandomWaypoints();
            }
        }

        if (agent.hasPath)
        {
            Vector3 direction = agent.steeringTarget - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
   

    void FollowRandomWaypoints()
    {
        if (waypoints.Length == 0)
            return;

        if (Vector3.Distance(transform.position, agent.destination) < 1)
        {
            randomWaypointCount--;
            if (randomWaypointCount <= 0)
            {
                allerVersWpFinal = true;
                waypointFinal = waypoints[Random.Range(0, waypoints.Length)];
                agent.SetDestination(waypointFinal.transform.position);
            }
            else
            {
                currentWP = GetRandomWaypointIndexExcludingFinal();
                agent.SetDestination(waypoints[currentWP].transform.position);
            }
        }
    }

    int GetRandomWaypointIndexExcludingFinal()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, waypoints.Length);
        } while (waypoints[randomIndex] == waypointFinal);
        return randomIndex;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collided with: " + hit.gameObject.name);
    }
}