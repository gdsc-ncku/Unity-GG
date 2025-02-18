using UnityEngine;
namespace FSM
{
    public class FSM : MonoBehaviour
    {
        [SerializeField]State initialState;
        public IState currentState { get; private set; }
        void Start()
        {
            SetState(initialState);
        }
        void Update()
        {
            currentState?.OnStateUpdate(this);
        }
        public void SetState(IState state)
        {
            currentState?.OnStateExit(this);
            currentState = state;
            currentState.OnStateEnter(this);
        }
    }
}