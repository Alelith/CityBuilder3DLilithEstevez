using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private GameObject[] goalLocations;
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        goalLocations = GameObject.FindGameObjectsWithTag("Goal");
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);

        //Animation
        anim = GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        ResetAgent();
    }

    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            ResetAgent();
            if (goalLocations != null)
                agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }

    private void ResetAgent()
    {
        float ms = Random.Range(0.5f, 0.75f);
        anim.SetFloat("multSpeed", ms);
        agent.speed *= ms;
        anim.SetTrigger("walk");
        agent.angularSpeed = 120;
        agent.ResetPath();
    }
}
