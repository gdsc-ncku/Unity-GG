using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 處理敵人移動
/// </summary>
/// TODO：把走預定路徑和追蹤目標的邏輯分開(可能會有不巡邏的怪)
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]NavMeshAgent agent;
    [SerializeField]GameObject path;
    Transform[] pathPoints;
    Transform _target;
    public Transform target
    {
        get => _target;
        set
        {
            _target = value;
        }
    }
    int currentPathPoint = 0;

    void Awake()
    {
        InitPath();
    }

    void Update()
    {
        if (target == null)
        {
            CheckPathPointReached();
        }
    }

    /// <summary>
    /// 初始化路徑
    /// </summary>
    void InitPath()
    {
        // 防止路徑跟著敵人走
        if (path != null)
        {
            path.transform.parent = null;
        }
        int pathPointNum = path.transform.childCount;
        pathPoints = new Transform[pathPointNum];
        for(int i = 0; i < pathPointNum; i++)
        {
            pathPoints[i] = path.transform.GetChild(i);
        }
    }

    /// <summary>
    /// 更新路徑
    /// </summary>
    void updatePath()
    {
        int nextIndex = (currentPathPoint + 1) % pathPoints.Length;
        currentPathPoint = nextIndex;
    }

    /// <summary>
    /// 移動到指定位置
    /// </summary>
    /// <param name="position"></param>
    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }
    void CheckPathPointReached()
    {
        if (IsArrived())
        {
            OnPathPointReached();
        }
    }
    void OnPathPointReached()
    {
        updatePath();
        MoveTo(pathPoints[currentPathPoint].position);
    }

    /// <summary>
    /// 判定是否到達目的地
    /// </summary>
    /// <returns></returns>
    bool IsArrived()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            return true;
        }
        return false;
    }
}
