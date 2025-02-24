using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine
    {
        StateNode currentNode;
        Dictionary<Type, StateNode> nodes = new();

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                SetState(transition.NextState);
            }
            currentNode.State.OnStateUpdate();
        }

        public void SetState(IState state)
        {
            currentNode?.State.OnStateExit();
            currentNode = GetNode(state);
            currentNode.State.OnStateEnter();
            Debug.Log($"currentState: {currentNode.State}");
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