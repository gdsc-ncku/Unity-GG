using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物件的類別
/// 屬於掉落物 還是 道具
/// </summary>
public enum ItemType
{
    Drop,
    Item
}

/// <summary>
/// 物件的名稱
/// </summary>
public enum ItemName
{
    MatrixEyes,
    Reducer,
    HoundExclusiveCameraModule,
    HoundExclusiveMemory,
    HoundExclusiveMicrophone,
    HoundExclusiveProcessorSoC
}

/// <summary>
/// 道具和掉落物的資料 腳本物件
/// </summary>
[CreateAssetMenu(fileName = "NewMaterial", menuName = "Crafting/Material")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public ItemName itemEnumName;
    public string itemName; //物品名稱
    public Sprite itemSprite;   //物品圖片

    public ItemType itemType;   //物品的類別
}
