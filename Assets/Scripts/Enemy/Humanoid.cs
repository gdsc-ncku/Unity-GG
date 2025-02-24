using FSM;
using UnityEngine;

public class Humanoid : EnemyBase
{
    protected override void Init()
    {
        var patrol = new Patrol(this);
        var chase = new Chase(this);
        var attack = new Attack(this);
        var flee = new Flee(this);
        var find = new Find(this);

        stateMachine = new StateMachine(patrol);

        stateMachine.AddTransition(patrol, chase, new FuncCondition(() => relation == Relation.Hate));
        stateMachine.AddTransition(patrol, flee, new FuncCondition(() => relation == Relation.Affraid));

        stateMachine.AddTransition(flee, patrol, new FuncCondition(() => OutOfFleeRange(flee.fleeFromTarget)));

        stateMachine.AddTransition(chase, attack, new FuncCondition(() => CanAttack(Target)));
        stateMachine.AddTransition(chase, flee, new FuncCondition(() => relation == Relation.Affraid));
        stateMachine.AddTransition(chase, find, new FuncCondition(() =>relation == Relation.Neutral));

        stateMachine.AddTransition(attack, chase, new FuncCondition(() => !CanAttack(Target)));
    }
    protected override void Attack()
    {
        base.Attack();
    }
}
