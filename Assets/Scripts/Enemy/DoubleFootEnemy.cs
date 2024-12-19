using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 雙腳怪物的狀態
/// </summary>
public enum DoubleFootEnemyStatus
{
    Patroling,
    Hunting
}

public class DoubleFootEnemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public DoubleFootEnemyStatus status;

    [Header("巡邏相關")]
    public GameObject path;
    private Transform[] pathPoint;
    [SerializeField] private bool isArrived = false;
    private int nowTrackingPoint = 0;
    private float distanceError = 1f;

    [Header("視野與狩獵")]
    public EnemyView enemyView;
    private GameObject[] scanObj = new GameObject[1]; //大小只有1 因為玩家只會有一個
    private float locationTimer = 0;
    public float locationCD = 0.5f;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        //載入巡邏路徑
        int pathPointNum = path.transform.childCount;
        pathPoint = new Transform[pathPointNum];
        for(int i = 0; i < pathPointNum; i++)
        {
            pathPoint[i] = path.transform.GetChild(i);
        }

        //初始化狀態
        status = DoubleFootEnemyStatus.Patroling;
        isArrived = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(status == DoubleFootEnemyStatus.Patroling)
        {
            if(enemyView.Filter(scanObj, "Player") > 0)
            {
                //狩獵模式
                status = DoubleFootEnemyStatus.Hunting;
                isArrived = true;
                locationTimer = locationCD;
            }

            Patroling();
        }else if(status == DoubleFootEnemyStatus.Hunting)
        {
            if (enemyView.Filter(scanObj, "Player") == 0)
            {
                //巡邏模式
                status = DoubleFootEnemyStatus.Patroling;
                isArrived = true;
            }

            Hunting();
        }
    }

    private void Hunting()
    {
        if(locationTimer >= locationCD)
        {
            agent.SetDestination(scanObj[0].transform.position);
            locationTimer = 0f;
        }
        else if(locationTimer < locationCD)
        {
            locationTimer += Time.deltaTime;
        }
    }

    private void Patroling()
    {
        if(isArrived == true)
        {
            //前進下一個巡邏點
            int nextIndex = (nowTrackingPoint + 1) % pathPoint.Length;
            agent.SetDestination(pathPoint[nextIndex].position);
            nowTrackingPoint = nextIndex;
            isArrived = false;
        } 

        if(Vector3.Distance(this.transform.position, pathPoint[nowTrackingPoint].position) < distanceError)
        {
            isArrived = true;
        }
    }
}
