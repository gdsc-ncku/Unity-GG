using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Layer detect", story: "[Target] [priority] is equal to [stay]", category: "Conditions", id: "4366e2f663233dd68531325b39cf82a8")]
public partial class LayerDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<State> Priority;
    [SerializeReference] public BlackboardVariable<State> Stay;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
