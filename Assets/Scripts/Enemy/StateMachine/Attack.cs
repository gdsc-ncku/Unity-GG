using UnityEngine;

public class Attack : EnemyState
{
    public Attack(EnemyBase enemy) : base(enemy) { }
    public override void OnStateEnter()
    {
        enemy.Attack();
    }
    public override void OnStateUpdate() { }
    public override void OnStateExit() { }
}
