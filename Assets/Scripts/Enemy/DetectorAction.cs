using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Detector", story: "[Detect] [target] then setting [targetInSight] and store [lastPosition] and [disappearBoolean]", category: "Action", id: "949b5333eed44a88741a81d97a3e888a")]
public partial class DetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<Detector> Detect;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<bool> TargetInSight;
    [SerializeReference] public BlackboardVariable<Vector3> LastPosition;
    [SerializeReference] public BlackboardVariable<bool> DisappearBoolean;
    protected override Status OnUpdate()
    {
        LayerMask otherEnemy = LayerTagPack.EnemyLayer, player = LayerMask.GetMask(LayerTagPack.Player);
        GameObject newTarget = Detect.Value.Detect(otherEnemy | player);
        if (newTarget != null)
        {
            LastPosition.Value = newTarget.transform.position;
            TargetInSight.Value = true;
            DisappearBoolean.Value = true;
        }
        else
        {
            TargetInSight.Value = false;
        }
        if (Target.Value != newTarget)
        {
            Target.Value = newTarget;
            return Status.Failure;
        }
        else
        {
            return Status.Success;
        }
    }

}

