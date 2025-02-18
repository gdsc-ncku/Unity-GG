using UnityEngine;

namespace FSM
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute(FSM machine);
    }
}