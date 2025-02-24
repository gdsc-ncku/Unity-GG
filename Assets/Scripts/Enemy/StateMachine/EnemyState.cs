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
    protected EnemyBase enemy;
    public EnemyState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }
    public virtual void OnStateEnter() { }
    public virtual void OnStateUpdate() { }
    public virtual void OnStateExit() { }
}