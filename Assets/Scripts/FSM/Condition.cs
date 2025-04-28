using System;

namespace FSM
{
    /// <summary>
    /// 定義條件
    /// </summary>
    public interface ICondition
    {
        bool Evaluate();
    }
    public class FuncCondition : ICondition
    {
        readonly Func<bool> func;
        public FuncCondition(Func<bool> func)
        {
            this.func = func;
        }
        public bool Evaluate() => func();
    }
}