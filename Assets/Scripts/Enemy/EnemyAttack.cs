using System;
using UniRx;
using UnityEngine;

public enum AttackType
{
    Electric,
    Hit,
}
public class EnemyAttack : MonoBehaviour
{
    Transform _target;
    public Transform target
    {
        get => _target;
        set
        {
            _target = value;
        }
    }
    [SerializeField]AttackType attackType;
    [SerializeField]float recoveryTime;
    public void Attack()
    {
        if (target == null) return;
        switch (attackType)
        {
            case AttackType.Electric:
                Debug.Log("Electric Attack");
                // 要開改變速度的接口
                break;
            case AttackType.Hit:
                Debug.Log("Hit Attack");
                // 要開玩家葛屁的接口
                break;
        }
        Recover();
    }
    void Recover()
    {
        Observable.Timer(TimeSpan.FromSeconds(recoveryTime)).Subscribe(_ => Attack()).AddTo(this);
    }
}
