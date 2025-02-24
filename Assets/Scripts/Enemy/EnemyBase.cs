using UnityEngine;
using FSM;
using UniRx;
using System.Linq;
using System.Collections.Generic;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]protected EnemyData enemyData;
    [SerializeField]protected Detector detector;
    public GameObject Target { get; private set; }
    protected StateMachine stateMachine;
    protected Dictionary<Faction, Relation> relations => enemyData.faction.GetRelations();
    protected Relation relation => relations.GetValueOrDefault(Target.GetFaction(), Relation.Neutral);

    void Start()
    {
        FactionManager.Register(gameObject, enemyData.faction);
        detector.OnDetectedChange.Subscribe(DecideTarget).AddTo(this);
        Init();
    }
    void Update()
    {
        stateMachine.Update();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init() { }

    protected bool CanAttack(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= enemyData.AttackRange;
    }
    
    /// <summary>
    /// 攻擊
    /// </summary>
    protected virtual void Attack() { }

    protected bool OutOfFleeRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) > enemyData.FleeRange;
    }

    /// <summary>
    /// 決定目標
    /// </summary>
    void DecideTarget(HashSet<GameObject> detectedObjects)
    {
        Debug.Log("Decide: " + detectedObjects.Count);
        Target = detectedObjects
                .Where(obj => obj != null)
                .Select(obj => new
                {
                    GameObject = obj,
                    RelationPriority = (int)relations.GetValueOrDefault(obj.GetFaction(), Relation.Neutral),
                    Distance = Vector3.Distance(transform.position, obj.transform.position)
                })
                .OrderBy(t => t.RelationPriority)
                .ThenBy(t => t.Distance)
                .Select(t => t.GameObject)
                .FirstOrDefault();
    }
    
    void OnDestroy()
    {
        FactionManager.Unregister(gameObject);
    }
}
