namespace FSM
{
    /// <summary>
    /// 定義狀態
    /// </summary>
    public interface IState
    {
        void OnStateEnter();
        void OnStateUpdate();
        void OnStateExit();
    }
    public class BaseState : IState
    {
        public virtual void OnStateEnter() { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateExit() { }
    }
}