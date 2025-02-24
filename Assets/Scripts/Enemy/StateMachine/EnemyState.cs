using FSM;

public enum State
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Flee,
    Find
}
public class EnemyState : IState
{
    public EnemyState(EnemyBase enemy) { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }
}