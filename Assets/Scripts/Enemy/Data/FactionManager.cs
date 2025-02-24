using System.Collections.Generic;
using UnityEngine;

public static class FactionManager
{
    private static Dictionary<GameObject, Faction> factionCache = new();

    public static void Register(this GameObject obj, Faction faction)
    {
        if (!factionCache.ContainsKey(obj))
        {
            factionCache[obj] = faction;
        }
    }

    public static void Unregister(this GameObject obj)
    {
        factionCache.Remove(obj);
    }

    public static Faction GetFaction(this GameObject obj)
    {
        if (obj == null) return Faction.None;
        return factionCache.GetValueOrDefault(obj, Faction.None);
    }
}