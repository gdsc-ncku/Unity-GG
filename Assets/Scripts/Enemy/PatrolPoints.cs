using UnityEngine;
using UnityEngine.AI;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField]Transform[] points;
    private int currentIndex = 0;

    public bool HasReached(NavMeshAgent agent)
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            return true;
        }
        return false;
    }

    public Transform GetNext()
    {
        if (points.Length == 0)
            return null;

        currentIndex = (currentIndex + 1) % points.Length;
        return points[currentIndex];
    }
}
