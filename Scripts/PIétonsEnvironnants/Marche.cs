using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Marche : MonoBehaviour
{
    public GameObject[] premiersWP;
    public GameObject[] waypoints;
    public GameObject WP_ligne; // Waypoint où les piétons feront la queue
    public float speed = 1.2f;
    //private float speed = ToggleManager.Instance.pedestrianSpeed;
    public float rotationSpeed = 2; // Vitesse de rotation
    public int minRandomWaypoints = 1; // Nombre minimal de waypoints aléatoires avant de se diriger vers le waypoint final
    public int maxRandomWaypoints = 3; // Nombre maximal de waypoints aléatoires avant de se diriger vers le waypoint final

    private int currentWP = 0;
    private bool premiersWPCompleted = false;
    private bool allerVersWpFinal = false;
    private int randomWaypointCount = 0; // Compteur de waypoints aléatoires atteints
    private NavMeshAgent agent;
    private Animator anim;
    private bool waitingAtLigne = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.angularSpeed = rotationSpeed * 100;
        anim = GetComponent<Animator>();
        anim.speed = speed;

        if (premiersWP.Length == 0)
        {
            currentWP = Random.Range(0, waypoints.Length);
            premiersWPCompleted = true;
        }
        else
        {
            agent.SetDestination(premiersWP[currentWP].transform.position);
        }

        randomWaypointCount = Random.Range(minRandomWaypoints, maxRandomWaypoints + 1);
        anim.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!premiersWPCompleted)
        {
            FollowInitialWaypoints();
        }
        else
        {
            FollowRandomWaypoints();
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

    void FollowInitialWaypoints()
    {
        if (Vector3.Distance(transform.position, agent.destination) < 1)
        {
            currentWP++;
            if (currentWP >= premiersWP.Length)
            {
                premiersWPCompleted = true;
                if (waypoints.Length > 0)
                {
                    currentWP = Random.Range(0, waypoints.Length);
                    agent.SetDestination(waypoints[currentWP].transform.position);
                }
            }
            else
            {
                agent.SetDestination(premiersWP[currentWP].transform.position);
            }
        }
    }

    void FollowRandomWaypoints()
    {
        if (waypoints.Length == 0 || waitingAtLigne)
            return;

        if (Vector3.Distance(transform.position, agent.destination) < 1)
        {
            if (waypoints[currentWP] == WP_ligne)
            {
                StartCoroutine(WaitAtLigne());
            }
            else
            {
                randomWaypointCount--;
                if (randomWaypointCount <= 0)
                {
                    allerVersWpFinal = true;
                    GameObject waypointFinal = waypoints[Random.Range(0, waypoints.Length)];
                    agent.SetDestination(waypointFinal.transform.position);
                }
                else
                {
                    currentWP = GetRandomWaypointIndexExcludingLigne();
                    agent.SetDestination(waypoints[currentWP].transform.position);
                }
            }
        }
    }

    IEnumerator WaitAtLigne()
    {
        waitingAtLigne = true;
        anim.SetBool("isWalking", false);
        agent.isStopped = true;
        yield return new WaitForSeconds(3);
        anim.SetBool("isWalking", true);
        agent.isStopped = false;
        waitingAtLigne = false;

        // Continuer vers le prochain waypoint après avoir attendu
        currentWP = GetRandomWaypointIndexExcludingLigne();
        agent.SetDestination(waypoints[currentWP].transform.position);
    }

    int GetRandomWaypointIndexExcludingLigne()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, waypoints.Length);
        } while (waypoints[randomIndex] == WP_ligne);
        return randomIndex;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collided with: " + hit.gameObject.name);
    }
}   