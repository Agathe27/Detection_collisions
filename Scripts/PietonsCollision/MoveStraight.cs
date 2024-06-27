using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UXF;
using TMPro;

public class MoveStraight : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private float moveTime;
    private float speed;
    private GameObject uiPanel;
    private Vector3 destination;
    //private Vector3 positionInitiale;
    private float stopDuration = 1.5f;
    private System.Action<GameObject> onStopCallback;
    //private KeyPressHandler keypresshandler; 

    public void Initialize(GameObject uiPanel, float moveTime, float speed, Vector3 destination, System.Action<GameObject> onStopCallback)
    {
        this.uiPanel = uiPanel;
        this.moveTime = moveTime;
        this.speed = speed;
        this.destination = destination;
        //this.positionInitiale = transform.position; 
        this.onStopCallback = onStopCallback;
        //this.keypresshandler = keyPressHandler; 

        // Initialiser les composants et démarrer la coroutine
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null && animator != null)
        {
            agent.speed = speed;
            StartCoroutine(MoveAgent());
        }
        else
        {
            Debug.LogError("NavMeshAgent or Animator component is missing!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.B))
        {
            HideUIPanel();
        }
    }

    private IEnumerator MoveAgent()
    {
        animator.SetBool("isWalking", true);
        agent.SetDestination(destination);

        yield return new WaitForSeconds(moveTime);

        animator.SetBool("isWalking", false);
        agent.isStopped = true;

        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            yield return new WaitForSeconds(stopDuration);
            /*if(keypresshandler!= null)
            {
                keypresshandler.EnregistrerData();
            }*/
            HideUIPanel();
        }

        onStopCallback?.Invoke(gameObject);
        //resetPedestrian();
    }

   /* private void resetPedestrian()
    {
        transform.position = positionInitiale;
        gameObject.SetActive(false); 
    }*/

    private void HideUIPanel()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }
}