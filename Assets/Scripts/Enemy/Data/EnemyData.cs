using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    // 敵人名稱
    public string enemyName => name;
    // 陣營
    public Faction faction;
    // 陣營關係
    public Dictionary<Faction, Relation> relations => faction.GetRelations();
    // 攻擊種類
    public AttackType attackType;
}
