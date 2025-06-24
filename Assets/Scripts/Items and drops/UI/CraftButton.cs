using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掛在於合成物品按鈕上的腳本
/// 用於觸發合成該物品的功能
/// </summary>
public class CraftButton : MonoBehaviour
{
    //物品資料
    public ItemData itemData;

    /// <summary>
    /// 觸發合成
    /// </summary>
    public void Craft()
    {
        Debug.Log($"CraftButton: try crafting {itemData.itemEnumName}");
        CraftManager.Instance.TryCraft(itemData.itemEnumName);
    }
}

