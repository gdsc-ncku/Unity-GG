using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// �I�]�޲z��
/// �t�d���@�R�A���I�]�귽
/// �B�z���޿�u�� �ӹD��O�_���¦s�b�ƶq����
/// ���ڪ��D���޿�L��
/// </summary>
public class InventoryManager : MonoBehaviour
{

    #region �إ߳�ҼҦ�
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

    #region ��l��
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


    [Header("�I�]")]
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    /// <summary>
    ///�V�I�]�K�[�D��
    /// </summary>
    /// <param name="item">�D��</param>
    /// <param name="quantity">�ƶq</param>
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
    /// �I�]�ݽT�{�O�_����ϥιD��
    /// </summary>
    /// <param name="item">�D��</param>
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
    /// ��ܷ�e�I�]�����D��
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
    /// �d�߫��w���~���ƶq�O�_����
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
    /// ���ӫ��w�ƶq�����~
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
