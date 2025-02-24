using FSM;

public class Humanoid : EnemyBase
{
    protected override void Init()
    {
        var patrol = new Patrol(this);
        var chase = new Chase(this);
        var attack = new Attack(this);
        var flee = new Flee(this);
        var lookAround = new LookAround(this);

        stateMachine.AddTransition(patrol, chase, new FuncCondition(() => relation == Relation.Hate));
        stateMachine.AddTransition(chase, attack, new FuncCondition(() => CanAttack(Target)));
        stateMachine.AddTransition(attack, chase, new FuncCondition(() => !CanAttack(Target)));
        stateMachine.AddTransition(chase, lookAround, new FuncCondition(() => relation == Relation.None));
        stateMachine.AddTransition(lookAround, chase, new FuncCondition(() => relation == Relation.Hate));
        stateMachine.AddTransition(lookAround, patrol, new FuncCondition(() => lookAround.IsFinished));
        stateMachine.AddTransition(chase, flee, new FuncCondition(() => relation == Relation.Affraid));

        stateMachine.AddTransition(patrol, flee, new FuncCondition(() => relation == Relation.Affraid));
        stateMachine.AddTransition(flee, patrol, new FuncCondition(() => !IsInFleeRange(flee.FleeFromTarget)));

        stateMachine.SetState(patrol);
    }
    public override void Attack()
    {
        base.Attack();
    }
}
