using UnityEngine;
using FSM;
using UniRx;
using System.Linq;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]protected EnemyData enemyData;
    [SerializeField]protected Detector detector;
    protected Dictionary<Faction, Relation> relations => enemyData.faction.GetRelations();
    protected StateMachine stateMachine;
    public GameObject Target { get; private set; }
    public Relation Relation { get; private set; }

    void Start()
    {
        FactionManager.Register(gameObject, enemyData.faction);
        detector.OnDetectedChange.Subscribe(Decide).AddTo(this);
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init() { }

    /// <summary>
    /// 決定目標
    /// </summary>
    void Decide(HashSet<GameObject> detectedObjects)
    {
        Debug.Log("Decide: " + detectedObjects.Count);
        Target = detectedObjects
                .Where(obj => obj != null)
                .Select(obj => new
                {
                    GameObject = obj,
                    RelationPriority = (int)relations.GetValueOrDefault(FactionManager.GetFaction(obj), Relation.Neutral),
                    Distance = Vector3.Distance(transform.position, obj.transform.position)
                })
                .OrderBy(t => t.RelationPriority)
                .ThenBy(t => t.Distance)
                .Select(t => t.GameObject)
                .FirstOrDefault();
        if (Target == null)
        {
            Relation = Relation.Neutral;
            return;
        }
        Relation = relations.GetValueOrDefault(FactionManager.GetFaction(Target), Relation.Neutral);
    }
    
    void OnDestroy()
    {
        FactionManager.Unregister(gameObject);
    }
}
