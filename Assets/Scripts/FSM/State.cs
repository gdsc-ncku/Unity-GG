using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/State")]
    public sealed class State : BaseState
    {
        public List<Action> Actions = new(); // 當前狀態的動作
        public List<Transition> Transitions = new(); // 當前狀態的轉換條件

        public override void OnStateUpdate(FSM machine)
        {
            base.OnStateUpdate(machine);
            foreach (var action in Actions)
            {
                action.Execute(machine);
            }
            foreach (var transition in Transitions)
            {
                transition.Execute(machine);
            }
        }
    }

    [CreateAssetMenu(menuName = "FSM/RemainInState")]
    public sealed class RemainInState : BaseState
    {
        // 甚麼都不做
    }

    public abstract class BaseState :  ScriptableObject, IState
    {
        public virtual void OnStateEnter(FSM machine)
        {
            Debug.Log($"Enter State: {name}");
        }

        public virtual void OnStateUpdate(FSM machine)
        {
            Debug.Log($"Update State: {name}");
        }

        public virtual void OnStateExit(FSM machine)
        {
            Debug.Log($"Exit State: {name}");
        }
    }
    
    public interface IState
    {
        void OnStateEnter(FSM machine);
        void OnStateUpdate(FSM machine);
        void OnStateExit(FSM machine);
    }
}