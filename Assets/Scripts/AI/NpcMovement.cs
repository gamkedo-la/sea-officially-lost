using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour 
{
	public GameObject targetGO;
	private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        HeadForDestination();
    }

    private void HeadForDestination()
    {
        Vector3 destination = targetGO.transform.position;
        navMeshAgent.SetDestination(destination);
    }
}
