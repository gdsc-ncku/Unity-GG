using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 雙腳怪物的狀態
/// </summary>
public enum EnemyStatus
{
    Patroling,
    Hunting
}

public class Enemy : MonoBehaviour
{
    EnemyStatus status;
    public NavMeshAgent agent;
    [Header("Patrolling")]
    [SerializeField]GameObject path;
    Transform[] pathPoint;
    bool isArrived;
    int nowTrackingPoint = 0;
    const float DISTACE_ERROR = 1f;

    [Header("View")]
    [SerializeField]EnemyView enemyView;
    GameObject player;

    [Header("Huning")]
    private float locationTimer = 0;
    public float locationCD = 0.5f;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        InitPath();
        status = EnemyStatus.Patroling;
        isArrived = true;
    }

    /// <summary>
    /// 載入巡邏路徑
    /// </summary>
    void InitPath()
    {
        int pathPointNum = path.transform.childCount;
        pathPoint = new Transform[pathPointNum];
        for(int i = 0; i < pathPointNum; i++)
        {
            pathPoint[i] = path.transform.GetChild(i);
        }
    }
    void Update()
    {
        if(status == EnemyStatus.Patroling)
        {
            if(enemyView.IsViewPlayer(out player) == true)
            {
                //狩獵模式
                status = EnemyStatus.Hunting;
                isArrived = true;
                locationTimer = locationCD;
            }
            Patroling();
        }
        else if(status == EnemyStatus.Hunting)
        {
            if (enemyView.IsViewPlayer(out player) == false)
            {
                //巡邏模式
                status = EnemyStatus.Patroling;
                isArrived = true;
            }
            if (player != null)
                Hunting(player);
        }
    }

    public virtual void Patroling()
    {
        if(isArrived == true)
        {
            //前進下一個巡邏點
            int nextIndex = (nowTrackingPoint + 1) % pathPoint.Length;
            agent.SetDestination(pathPoint[nextIndex].position);
            nowTrackingPoint = nextIndex;
            isArrived = false;
        } 

        if(Vector3.Distance(this.transform.position, pathPoint[nowTrackingPoint].position) < DISTACE_ERROR)
        {
            isArrived = true;
        }
    }

    public virtual void Hunting(GameObject player)
    {
        if(locationTimer >= locationCD)
        {
            agent.SetDestination(player.transform.position);
            locationTimer = 0f;
        }
        else if(locationTimer < locationCD)
        {
            locationTimer += Time.deltaTime;
        }
    }

    public virtual void Attacking()
    {

    }


    public virtual void FrindlyInteract() 
    { 

    }

    public virtual void HostileInteract()
    {

    }
}
