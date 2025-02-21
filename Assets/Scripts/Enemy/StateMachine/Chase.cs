using FSM;
using UnityEngine;
using UnityEngine.AI;
public class Chase : BaseState
{
    private NavMeshAgent navMeshAgent;
    private Transform target;

    public Chase(EnemyBase enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        target = enemy.target;
    }

    public override void OnStateUpdate()
    {
        if (target == null) return;
            navMeshAgent.SetDestination(target.position);
    }
}
