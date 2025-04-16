using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 統籌各腳本，可以視為敵人的大腦
/// </summary>
public class EnemyBase : MonoBehaviour
{
    [SerializeField]EnemyMovement movement;
    [SerializeField]EnemyAttack attack;
    [SerializeField]Detector searchableDetector;
    [SerializeField]Detector attackableDetector;
    string targetTag = LayerTagPack.Player;
    void Start()
    {
        searchableDetector.targetTag = targetTag;
        attackableDetector.targetTag = targetTag;
        searchableDetector.OnTargetGet.Subscribe(OnViewTarget).AddTo(this);
        searchableDetector.OnTargetGone.Subscribe(_ => OnTargetGone()).AddTo(this);
        attackableDetector.OnTargetGet.Subscribe(OnAttackable).AddTo(this);
    }
    void OnAttackable(GameObject player)
    {
        attack.target = player.transform;
        attack.Attack();
    }
    void OnViewTarget(GameObject player)
    {
        movement.target = player.transform;
        movement.MoveTo(player.transform.position);
    }
    void OnTargetGone()
    {
        movement.target = null;
    }
}
