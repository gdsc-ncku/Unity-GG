using UnityEngine;
using UnityEngine.AI;

public class Flee : EnemyState
{
    NavMeshAgent navMeshAgent;
    public GameObject fleeFromTarget { get; private set; }
    public Flee(EnemyBase enemy) : base(enemy)
    {
        fleeFromTarget = enemy.Target;
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }
    public override void OnStateEnter()
    {
        navMeshAgent.SetDestination(-fleeFromTarget.transform.position);
    }
}
