using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 道具需要用到Monobehavior
/// 實際的功能從這裡繼承
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData itemData;   //該物件的資料


    private Rigidbody body;

    public void Start()
    {
        Init();
    }

    /// <summary>
    /// 根據當前item的類別
    /// 進行不同的初始化
    /// 
    /// </summary>
    private void Init()
    {
        if(itemData.itemType == ItemType.Drop)
        {
            if (body == null)
            {
                body = this.gameObject.AddComponent<Rigidbody>();
            }
            body.useGravity = true;
        }
    }

    /// <summary>
    /// 如果是掉落物可以撿取
    /// </summary>
    public void DropPicking()
    {
        Debug.Log($"Item: picking drop: {itemData.itemName}");
        if (itemData.itemType == ItemType.Drop)
        {
            InventoryManager.Instance.AddItem(itemData.itemEnumName);
            Destroy(this.gameObject);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if(itemData.itemType == ItemType.Drop && collision.gameObject.tag == "Player")
        {
            DropPicking();
            Debug.Log($"Item: {itemData.itemName} is touched by player");
        }


    }

    private void OnDestroy()
    {
        itemData = null; // 清理引用
    }
}
