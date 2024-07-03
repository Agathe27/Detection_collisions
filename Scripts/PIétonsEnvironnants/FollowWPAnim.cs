using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWPAnimation : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    public float speed = 3.0f;
    public float rotationSpeed = 2.0f;
    public float waypointThreshold = 0.4f; // Distance to consider reached waypoint

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // If there are no waypoints, return
        if (waypoints.Length == 0)
            return;

        // Check if the agent is close to the current waypoint
        if (Vector3.Distance(this.transform.position, waypoints[currentWP].transform.position) < waypointThreshold)
        {
            currentWP++;
            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
        }

        // Move towards the current waypoint
        Vector3 direction = (waypoints[currentWP].transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Rotate towards the waypoint : rotation plus fluide 
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move forward
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update animation based on movement
        if (direction.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}