using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成圖的腳本物件
/// </summary>
[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
[System.Serializable]
public class Recipe : ScriptableObject
{
    public bool isUnlocked; //玩家是否解鎖該配方
    public List<ItemData> requiredMaterials; //合成需要的材料
    public ItemData resultItem;         //合成的結果
}
