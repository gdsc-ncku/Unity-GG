using System;
using System.Collections.Generic;

namespace FSM
{
    public class StateMachine
    {
        StateNode currentNode;
        Dictionary<Type, StateNode> nodes = new();
        public StateMachine(IState initialState)
        {
            currentNode = new(initialState);
        }

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                SetState(transition.NextState);
            }
            currentNode.State?.OnStateUpdate();
        }
        
        public void SetState(IState state)
        {
            currentNode.State?.OnStateExit();
            currentNode = nodes[state.GetType()];
            currentNode.State.OnStateEnter();
        }

        public void AddTransition(IState fromState, IState nextState, ICondition condition)
        {
            GetNode(fromState).AddTransition(GetNode(nextState).State, condition);
        }

        StateNode GetNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        ITransition GetTransition()
        {
            foreach (var transition in currentNode.Transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }
            return null;
        }
    }
}