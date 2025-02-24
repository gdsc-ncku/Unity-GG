using FSM;

public class Attack : EnemyState
{
    public Attack(EnemyBase enemy) : base(enemy) { }
    public override void OnStateEnter() { }
    public override void OnStateUpdate() { }
    public override void OnStateExit() { }
}
