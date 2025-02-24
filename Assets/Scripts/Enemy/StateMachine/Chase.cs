using FSM;
using UnityEngine;
using UnityEngine.AI;
public class Chase : EnemyState
{
    private NavMeshAgent navMeshAgent;
    private GameObject target;

    public Chase(EnemyBase enemy) : base(enemy)
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
