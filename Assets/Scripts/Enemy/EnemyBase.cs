using UnityEngine;
using FSM;
using UniRx;
using System.Linq;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]protected EnemyData enemyData;
    [SerializeField]protected Detector detector;
    public GameObject Target { get; private set; }
    protected StateMachine stateMachine = new();
    protected Dictionary<Faction, Relation> relations => enemyData.faction.GetRelations();
    protected Relation relation => relations.GetValueOrDefault(FactionManager.Instance.GetFaction(Target), Relation.None);

    void Start()
    {
        FactionManager.Instance.Register(gameObject, enemyData.faction);
        detector.OnDetectedChange.Subscribe(DecideTarget).AddTo(this);
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init() { }

    void Update()
    {
        stateMachine.Update();
    }

    protected bool CanAttack(GameObject target)
    {
        if (target == null) return false;
        return IsInAttackRange(target);
    }

    bool IsInAttackRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= enemyData.AttackRange;
    }
    
    /// <summary>
    /// 攻擊
    /// </summary>
    public virtual void Attack() { }

    protected bool IsInFleeRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= enemyData.FleeRange;
    }

    /// <summary>
    /// 決定目標
    /// </summary>
    void DecideTarget(HashSet<GameObject> detectedObjects)
    {
        Target = detectedObjects
                .Where(obj => obj != null)
                .Select(obj => new
                {
                    GameObject = obj,
                    RelationPriority = (int)relations.GetValueOrDefault(FactionManager.Instance.GetFaction(obj), Relation.None),
                    Distance = Vector3.Distance(transform.position, obj.transform.position)
                })
                .OrderBy(t => t.RelationPriority)
                .ThenBy(t => t.Distance)
                .Select(t => t.GameObject)
                .FirstOrDefault();
        Debug.Log($"{Target?.name}: {relation}");
    }
    
    void OnDestroy()
    {
        FactionManager.Instance.Unregister(gameObject);
    }
}
