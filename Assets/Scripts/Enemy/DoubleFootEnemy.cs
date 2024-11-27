using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���}�Ǫ������A
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

    [Header("���ެ���")]
    public GameObject path;
    private Transform[] pathPoint;
    [SerializeField] private bool isArrived = false;
    private int nowTrackingPoint = 0;
    private float distanceError = 1f;

    [Header("�����P���y")]
    public EnemyView enemyView;
    private GameObject[] scanObj = new GameObject[1]; //�j�p�u��1 �]�����a�u�|���@��
    private float locationTimer = 0;
    public float locationCD = 0.5f;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        //���J���޸��|
        int pathPointNum = path.transform.childCount;
        pathPoint = new Transform[pathPointNum];
        for(int i = 0; i < pathPointNum; i++)
        {
            pathPoint[i] = path.transform.GetChild(i);
        }

        //��l�ƪ��A
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
                //���y�Ҧ�
                status = DoubleFootEnemyStatus.Hunting;
                isArrived = true;
                locationTimer = locationCD;
            }

            Patroling();
        }else if(status == DoubleFootEnemyStatus.Hunting)
        {
            if (enemyView.Filter(scanObj, "Player") == 0)
            {
                //���޼Ҧ�
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
            //�e�i�U�@�Ө����I
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
