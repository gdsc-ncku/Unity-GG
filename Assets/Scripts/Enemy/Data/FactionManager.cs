using System.Collections.Generic;
using UnityEngine;

public static class FactionManager
{
    private static Dictionary<GameObject, Faction> factionCache = new();

    public static void Register(GameObject obj, Faction faction)
    {
        if (!factionCache.ContainsKey(obj))
        {
            factionCache[obj] = faction;
        }
    }

    public static void Unregister(GameObject obj)
    {
        factionCache.Remove(obj);
    }

    public static Faction GetFaction(this GameObject obj)
    {
        return factionCache.GetValueOrDefault(obj, Faction.None);
    }
}