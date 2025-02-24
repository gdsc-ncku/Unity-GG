using UnityEngine;
using UnityEngine.AI;

public class Flee : EnemyState
{
    NavMeshAgent navMeshAgent;
    public GameObject FleeFromTarget { get; private set; }
    public Flee(EnemyBase enemy) : base(enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }
    public override void OnStateEnter()
    {
        FleeFromTarget = enemy.Target;
    }
    public override void OnStateUpdate()
    {
        if (FleeFromTarget == null) return;
        Vector3 fleeDirection = enemy.transform.position - FleeFromTarget.transform.position;
        navMeshAgent.SetDestination(enemy.transform.position + fleeDirection.normalized);
    }
}
