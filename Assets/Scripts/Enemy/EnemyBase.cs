using UnityEngine;
using FSM;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]protected EnemyData enemyData;
    [SerializeField]protected Detector detector;
    protected StateMachine stateMachine;
    public Transform target;
}
