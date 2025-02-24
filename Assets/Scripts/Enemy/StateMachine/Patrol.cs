using UnityEngine.AI;
using FSM;

public class Patrol : EnemyState
{
    NavMeshAgent navMeshAgent;
    PatrolPoints patrolPoints;
    public Patrol(EnemyBase enemy) : base(enemy)
    {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        patrolPoints = enemy.GetComponent<PatrolPoints>();
    }
    public override void OnStateUpdate()
    {
        if (patrolPoints.HasReached(navMeshAgent))
            navMeshAgent.SetDestination(patrolPoints.GetNext().position);
    }
}