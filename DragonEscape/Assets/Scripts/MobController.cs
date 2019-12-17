using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    [SerializeField] private List<Vector3> targetPointPositoinList;
    private Vector3 targetPositoin;//目標地点

    private NavMeshAgent agent;
    private int next; 

    private void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        next++;
    }

    private void Update()
    {
        if (agent.remainingDistance<=1.0f)
        {
            if (next >= targetPointPositoinList.Capacity)
            {
                next = 0;
                targetPositoin = targetPointPositoinList[next];
            }
            else
            {
                targetPositoin = targetPointPositoinList[next];
                next++;
            }
            this.agent.destination = targetPositoin;

        }
        else
        {
            targetPositoin = targetPointPositoinList[next];
        }
    }
}
