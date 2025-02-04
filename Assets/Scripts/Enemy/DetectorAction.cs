using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Detector", story: "[Detect] [enemy] and [player] then setting [playerInSight] and [enemyInSight]", category: "Action", id: "949b5333eed44a88741a81d97a3e888a")]
public partial class DetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<Detector> Detect;
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<bool> PlayerInSight;
    [SerializeReference] public BlackboardVariable<bool> EnemyInSight;
    protected override Status OnUpdate()
    {
        Enemy.Value = Detect.Value.Detect(LayerMask.GetMask(LayerTagPack.Enemy));
        EnemyInSight.Value = (Enemy.Value != null);
        Player.Value = Detect.Value.Detect(LayerMask.GetMask(LayerTagPack.Player));
        PlayerInSight.Value = (Player.Value != null);
        return (EnemyInSight.Value || PlayerInSight.Value) ? Status.Success : Status.Failure;
    }

}

