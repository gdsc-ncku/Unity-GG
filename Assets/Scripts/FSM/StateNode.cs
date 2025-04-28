using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// 狀態節點: 結合狀態﹑轉換﹑條件
    /// </summary>
    public class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }
        public StateNode(IState state)
        {
            State = state;
            Transitions = new();
        }
        public void AddTransition(IState nextState, ICondition condition)
        {
            Transitions.Add(new Transition(nextState, condition));
        }
    }
}
