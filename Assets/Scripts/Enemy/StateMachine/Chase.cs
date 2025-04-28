using FSM;
using UnityEngine;
using UnityEngine.AI;
public class Chase : EnemyState
{
    NavMeshAgent navMeshAgent;
    GameObject target => enemy.Target;

    public Chase(EnemyBase enemy) : base(enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate()
    {
        if (target == null) return;
        navMeshAgent.SetDestination(target.transform.position);
    }
}
