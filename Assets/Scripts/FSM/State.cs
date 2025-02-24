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
}