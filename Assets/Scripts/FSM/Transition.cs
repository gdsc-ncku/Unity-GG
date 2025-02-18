using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public sealed class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public void Execute(FSM stateMachine)
        {
            if(Decision.Decide(stateMachine) && !(TrueState is RemainInState))
                stateMachine.SetState(TrueState);
            else if(!(FalseState is RemainInState))
                stateMachine.SetState(FalseState);
        }
    }
}