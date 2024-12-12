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
    [SerializeField]Detector detector;
    string targetTag = LayerTagPack.Player;
    void Start()
    {
        detector.targetTag = targetTag;
        detector.OnViewTarget.Subscribe(OnViewTarget).AddTo(this);
        detector.OnTargetGone.Subscribe(_ => OnTargetGone()).AddTo(this);
    }

    void OnViewTarget(GameObject player)
    {
        movement.target = player.transform;
    }
    void OnTargetGone()
    {
        movement.target = null;
    }
}
