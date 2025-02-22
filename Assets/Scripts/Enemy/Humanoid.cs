using FSM;

public class Humanoid : EnemyBase
{
    protected override void Init()
    {
        var Patrol = new Patrol(this);
        var Chase = new Chase(this);

        stateMachine = new StateMachine(Patrol);
        // stateMachine.AddTransition(Patrol, Chase, new FuncCondition(() => Vector3.Distance(transform.position, target.position) < enemyData.detectDistance));
    }
}
