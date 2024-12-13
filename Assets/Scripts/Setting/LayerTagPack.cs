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
        public const string Enemy = "Enemy";
        public const string BaseEnemy = "BaseEnemy";
        public const string MilitaryEnemy = "MilitaryEnemy";
        public const string Environment = "Environment";
}
