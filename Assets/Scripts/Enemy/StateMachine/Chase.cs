using FSM;
using UnityEngine;
using UnityEngine.AI;
public class Chase : BaseState
{
    private NavMeshAgent navMeshAgent;
    private GameObject target;

    public Chase(EnemyBase enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        target = enemy.Target;
    }

    public override void OnStateUpdate()
    {
        if (target == null) return;
            navMeshAgent.SetDestination(target.transform.position);
    }
}
