using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// 背包管理者
/// 負責維護靜態的背包資源
/// 處理的邏輯只有 該道具是否仍舊存在數量等等
/// 跟實際的道具邏輯無關
/// </summary>
public class InventoryManager : MonoBehaviour
{

    #region 建立單例模式
    //instance mode
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find Inventory Manager Instance");
            }
            return _instance;
        }

    }

    #endregion

    #region 初始化
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Duplicate creating Inventory Manager Instance");
            Destroy(gameObject);
        }
    }
    #endregion


    [Header("背包")]
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    /// <summary>
    ///向背包添加道具
    /// </summary>
    /// <param name="item">道具</param>
    /// <param name="quantity">數量</param>
    public void AddItem(ItemName itemName, int quantity = 1)
    {
        ItemData item = ItemManager.Instance.itemEnumName_itemsData_illustratedBook[itemName];

        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory[item] = quantity;
        }

        EventManager.TriggerEvent(NameOfEvent.InventoryItemChange);

        Debug.Log($"InventoryManager: Added {quantity} x {item.itemName} to inventory.");
    }

    /// <summary>
    /// 背包端確認是否能夠使用道具
    /// </summary>
    /// <param name="item">道具</param>
    public bool UseItem(ItemName itemName)
    {
        ItemData item = ItemManager.Instance.itemEnumName_itemsData_illustratedBook[itemName];

        if (inventory.ContainsKey(item) && inventory[item] > 0)
        {
            inventory[item]--;
            Debug.Log($"InventoryManager: Used 1 x {item.itemName}. Remaining: {inventory[item]}");

            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }

            EventManager.TriggerEvent(NameOfEvent.InventoryItemChange);

            return true;
        }
        else
        {
            Debug.LogWarning($"InventoryManager: No {item.itemName} left to use!");
            return false;
        }
    }

    /// <summary>
    /// 顯示當前背包中的道具
    /// </summary>
    public void ListInventory()
    {
        Debug.Log("InventoryManager: Player Inventory:");
        foreach (var entry in inventory)
        {
            Debug.Log($"- {entry.Key.itemName}: {entry.Value}");
        }
    }

    /// <summary>
    /// 查詢指定物品的數量是否滿足
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public bool SearchItem(ItemName itemName, int quantity = 1)
    {
        ItemData item = ItemManager.Instance.itemEnumName_itemsData_illustratedBook[itemName];

        if (inventory.ContainsKey(item) && inventory[item] >= quantity)
        {
            Debug.Log($"InventoryManager: Item Search {quantity} x {item.itemName} existed.");
            return true;
        }
        else
        {
            Debug.LogWarning($"InventoryManager: Item Search {quantity} x {item.itemName} failure.");
            return false;
        }
    }

    /// <summary>
    /// 消耗指定數量的物品
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public void ConsumeItem(ItemName itemName, int quantity = 1)
    {
        ItemData item = ItemManager.Instance.itemEnumName_itemsData_illustratedBook[itemName];

        if (inventory.ContainsKey(item) && inventory[item] >= quantity)
        {
            inventory[item]-= quantity;

            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
            Debug.Log($"InventoryManager: Item consume {quantity} x {item.itemName}.");

            EventManager.TriggerEvent(NameOfEvent.InventoryItemChange);
        }
        else
        {
            Debug.LogWarning($"InventoryManager: Item consume {quantity} x {item.itemName} failure.");
        }
    }
}
