namespace FSM
{
    /// <summary>
    /// 定義轉換
    /// </summary>
    public interface ITransition
    {
        IState NextState { get; }
        ICondition Condition { get; }
    }
    public class Transition : ITransition
    {
        public IState NextState { get; }
        public ICondition Condition { get; }

        public Transition(IState nextState, ICondition condition)
        {
            NextState = nextState;
            Condition = condition;
        }
    }
}