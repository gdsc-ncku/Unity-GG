using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerTagPack
{
    public static bool CompareTag(this GameObject gameObject, string tag)
    {
        return gameObject.tag == tag;
    }
    public static bool CompareLayer(this GameObject gameObject, string layer)
    {
        return gameObject.layer == LayerMask.NameToLayer(layer);
    }
    public const string Default = "Default";
    public const string TransparentFX = "TransparentFX";
    public const string IgnoreRaycast = "Ignore Raycast";
    public const string Water = "Water";
    public const string UI = "UI";
    public const string Player = "Player";
    public const string BaseEnemy = "BaseEnemy";
    public const string MilitaryEnemy = "MilitaryEnemy";
    public const string GPowerEnemy = "GPowerEnemy";
    public const string UndergroundDwellers = "UndergroundDwellers";
    public const string Environment = "Environment";
        
    static public LayerMask getEnemyLayer()
    {
        return LayerMask.GetMask(UndergroundDwellers, GPowerEnemy, MilitaryEnemy, BaseEnemy);
    }

    static public void initDiagrams(Dictionary<LayerMask, Dictionary<LayerMask, int>> diagram)
    {
        diagram[LayerMask.GetMask(Player)] = new();
        diagram[LayerMask.GetMask(BaseEnemy)] = new();
        diagram[LayerMask.GetMask(MilitaryEnemy)] = new();
        diagram[LayerMask.GetMask(GPowerEnemy)] = new();
        diagram[LayerMask.GetMask(UndergroundDwellers)] = new();
    }
}
