using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Three enemy behavior choosing", story: "Set next [state] according to [self] and [target] layer", category: "Action", id: "b75da2bbdfe7b3659ce15d78d57f2858")]
public partial class ThreeEnemyBehaviorChoosingAction : Action
{
    [SerializeReference] public BlackboardVariable<State> State;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    protected override Status OnUpdate()
    {
        if(Target.Value == null)
        {
            State.Value = global::State.Idle;
            return Status.Success;
        }
        string target = LayerMask.LayerToName(Target.Value.layer), self = LayerMask.LayerToName(Self.Value.layer);
        int diagramValue = EnemyDiagrams.enemyDiagram[LayerMask.GetMask(self)][LayerMask.GetMask(target)];
        switch(diagramValue)
        {
            case EnemyDiagrams.runAway:
                State.Value = global::State.RunAway;
                break;
            case EnemyDiagrams.stay:
                State.Value = global::State.Patroling;
                break;
            case EnemyDiagrams.attack:
                State.Value = global::State.Chasing;
                break;
        }
                
        return Status.Success;
    }
}

