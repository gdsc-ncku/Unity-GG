using System.Collections.Generic;

/// <summary>
/// 陣營
/// </summary>
public enum Faction
{
    None,
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
    None = 0,
    Hate = 1,
    Affraid = 2,
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
                { Faction.Player, Relation.Affraid },
                { Faction.MilitaryEnemy, Relation.Hate },
                { Faction.GPowerEnemy, Relation.Hate },
                { Faction.BaseEnemy, Relation.Hate },
                { Faction.UndergroundDwellers, Relation.None }
            }
        },
        {
            Faction.MilitaryEnemy, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Hate },
                { Faction.MilitaryEnemy, Relation.Affraid },
                { Faction.GPowerEnemy, Relation.None },
                { Faction.BaseEnemy, Relation.None },
                { Faction.UndergroundDwellers, Relation.Hate }
            }
        },
        {
            Faction.GPowerEnemy, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Hate },
                { Faction.MilitaryEnemy, Relation.Affraid },
                { Faction.GPowerEnemy, Relation.Affraid },
                { Faction.BaseEnemy, Relation.Affraid },
                { Faction.UndergroundDwellers, Relation.Affraid }
            }
        },
        {
            Faction.BaseEnemy, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.Hate },
                { Faction.MilitaryEnemy, Relation.Hate },
                { Faction.GPowerEnemy, Relation.Hate },
                { Faction.BaseEnemy, Relation.Hate },
                { Faction.UndergroundDwellers, Relation.Hate }
            }
        },
        {
            Faction.UndergroundDwellers, new Dictionary<Faction, Relation>
            {
                { Faction.Player, Relation.None },
                { Faction.MilitaryEnemy, Relation.Hate },
                { Faction.GPowerEnemy, Relation.Hate },
                { Faction.BaseEnemy, Relation.None },
                { Faction.UndergroundDwellers, Relation.Affraid }
            }
        }
    };

    public static Dictionary<Faction, Relation> GetRelations(this Faction a)
    {
        return relations[a];
    }
}