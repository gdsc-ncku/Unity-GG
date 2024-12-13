using UnityEngine;
using UnityEngine.AI;
using UniRx;

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
    bool isArrived = true;

    void Awake()
    {
        InitPath();
    }

    void Update()
    {
        // 無目標時，走預定路徑
        if (target == null)
        {
            MoveTo(pathPoints[currentPathPoint].position);
        }
        // 有目標時，追蹤目標
        else
        {
            agent.SetDestination(target.position);
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
    void MoveTo(Vector3 position)
    {
        if (IsArrived())
        {
            agent.SetDestination(position);
            updatePath();
        }
    }

    /// <summary>
    /// 判定是否到達目的地
    /// </summary>
    /// <returns></returns>
    bool IsArrived()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && isArrived)
        {
            return true;
        }
        return false;
    }
}
