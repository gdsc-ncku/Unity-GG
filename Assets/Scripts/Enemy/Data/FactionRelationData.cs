using System.Collections.Generic;

/// <summary>
/// 陣營
/// </summary>
public enum Faction
{
    Player,
    MilitaryEnemy,
    GPowerEnemy,
    BaseEnemy,
    UndergroundDwellers
}

/// <summary>
/// 陣營關係
/// </summary>
public enum Relation
{
    Friendly,
    Neutral,
    Hostile
}

/// <summary>
/// 陣營關係資料
/// NOTE: 這樣寫超醜，但用ScriptableObject又要一直拖元件
/// NOTE: 之後可以改成JSON試試
/// </summary>
public static class FactionRelationData
{
    static Dictionary<Faction, Dictionary<Faction, Relation>> relations = new()
    {
        {
            Faction.Player, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Friendly },
                { Faction.MilitaryEnemy, Relation.Hostile },
                { Faction.GPowerEnemy, Relation.Hostile },
                { Faction.BaseEnemy, Relation.Hostile },
                { Faction.UndergroundDwellers, Relation.Neutral }
            }
        },
        {
            Faction.MilitaryEnemy, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Hostile },
                { Faction.MilitaryEnemy, Relation.Friendly },
                { Faction.GPowerEnemy, Relation.Neutral },
                { Faction.BaseEnemy, Relation.Neutral },
                { Faction.UndergroundDwellers, Relation.Hostile }
            }
        },
        {
            Faction.GPowerEnemy, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Hostile },
                { Faction.MilitaryEnemy, Relation.Neutral },
                { Faction.GPowerEnemy, Relation.Friendly },
                { Faction.BaseEnemy, Relation.Neutral },
                { Faction.UndergroundDwellers, Relation.Hostile }
            }
        },
        {
            Faction.BaseEnemy, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Hostile },
                { Faction.MilitaryEnemy, Relation.Neutral },
                { Faction.GPowerEnemy, Relation.Neutral },
                { Faction.BaseEnemy, Relation.Friendly },
                { Faction.UndergroundDwellers, Relation.Neutral }
            }
        },
        {
            Faction.UndergroundDwellers, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Neutral },
                { Faction.MilitaryEnemy, Relation.Hostile },
                { Faction.GPowerEnemy, Relation.Hostile },
                { Faction.BaseEnemy, Relation.Neutral },
                { Faction.UndergroundDwellers, Relation.Friendly }
            }
        }
    };

    public static Dictionary<Faction, Relation> GetRelations(this Faction a)
    {
        return relations[a];
    }
}