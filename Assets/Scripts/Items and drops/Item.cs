using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具需要用到Monobehavior
/// 實際的功能從這裡繼承
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData itemData;   //該物件的資料


    /// <summary>
    /// 如果是掉落物可以撿取
    /// </summary>
    public void DropPicking()
    {
        Debug.Log($"picking drop: {itemData.itemName}");
    }

    /// <summary>
    /// 如果是道具 可以使用
    /// </summary>
    public virtual void ItemUsing()
    {
        Debug.Log($"using item: {itemData.itemName}");

        //TODO
        //持有數量-1
        //UI特效觸發
    }

    private void OnDestroy()
    {
        itemData = null; // 清理引用
    }
}
