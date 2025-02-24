using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Faction faction;
    public Weakness weakness;
    public float AttackRange;
    public float FleeRange;

    // // 偵測: 為了預覽效果，先由Detector設定
    // public float detectDistance;
    // public float detectAngle;

    // // 攻擊: 改用碰撞判定比較精準
    // public float attackDistance;
    // public float attackAngle;
}
public enum Weakness
{
    Head,
    Body,
    Leg
}