using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoSingleton<FactionManager>
{
    private Dictionary<GameObject, Faction> factionCache = new();

    public void Register(GameObject obj, Faction faction)
    {
        if (!factionCache.ContainsKey(obj))
        {
            factionCache[obj] = faction;
        }
    }

    public void Unregister(GameObject obj)
    {
        factionCache.Remove(obj);
    }

    public Faction GetFaction(GameObject obj)
    {
        if (obj == null) return Faction.None;
        return factionCache.GetValueOrDefault(obj, Faction.None);
    }
}