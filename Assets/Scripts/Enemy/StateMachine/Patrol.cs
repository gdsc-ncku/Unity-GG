using UnityEngine.AI;

namespace FSM
{
    // public class Patrol : BaseState
    // {
    //     NavMeshAgent navMeshAgent;
    //     PatrolPoints patrolPoints;
    //     public override void OnStateEnter(StateMachine stateMachine)
    //     {
    //         navMeshAgent = stateMachine.GetComponent<NavMeshAgent>();
    //         patrolPoints = stateMachine.GetComponent<PatrolPoints>();
    //     }
    //     public override void OnStateUpdate(StateMachine stateMachine)
    //     {
    //         if (patrolPoints.HasReached(navMeshAgent))
    //             navMeshAgent.SetDestination(patrolPoints.GetNext().position);
    //     }
    // }
}