using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 處理所有跟道具相關的事物
/// </summary>
public class ItemManager : MonoBehaviour
{
    #region 用於建立物件資料 對於 物件腳本的圖鑑
    public List<GameObject> itemPrefabs = new List<GameObject>();
    //[HideInInspector] public Dictionary<ItemData, GameObject> itemData_items_illustratedBook = new Dictionary<ItemData, GameObject>(); //所有的item資訊

    //item name 對應 prefab 遊戲物件的圖鑑
    [HideInInspector] public Dictionary<ItemName, GameObject> itemEnumName_itemsPrefabs_illustratedBook = new Dictionary<ItemName, GameObject>(); //所有的item資訊
    
    //item name 對應 item 資料的圖鑑
    [HideInInspector] public Dictionary<ItemName, ItemData> itemEnumName_itemsData_illustratedBook = new Dictionary<ItemName, ItemData>(); //所有的item資訊

    //item name 對應 item 效果腳本的圖鑑
    [HideInInspector] public Dictionary<ItemName, Item> itemEnumName_items_illustratedBook = new Dictionary<ItemName, Item>(); //所有的item資訊

    #endregion

    private ItemName choosed;

    #region 建立單例模式
    //instance mode
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find Item Manager Instance");
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
        
            //初始化圖鑑
            for(int i = 0; i < itemPrefabs.Count; i++)
            {
                GameObject obj = Instantiate(itemPrefabs[i], this.transform);//itemPrefabs[i];
                Item item = itemPrefabs[i].GetComponent<Item>();
                ItemData data = item.itemData;
                ItemName itemEnumName = data.itemEnumName;

                //關閉實體組件
                //只保留腳本效果
                obj.GetComponent<MeshRenderer>().enabled = false;
                obj.GetComponent<Collider>().enabled = false;

                //加入圖鑑
                //itemData_items_illustratedBook[data] = obj;
                itemEnumName_itemsPrefabs_illustratedBook[itemEnumName] = itemPrefabs[i];
                itemEnumName_itemsData_illustratedBook[itemEnumName] = data;
                itemEnumName_items_illustratedBook[itemEnumName] = item;
            }
        }
        else
        {
            Debug.LogError("Duplicate creating Item Manager Instance");
            Destroy(gameObject);
        }
    }
    #endregion

    /// <summary>
    /// 觸發道具
    /// </summary>
    public void ItemTrigger()
    {
        ItemData item = itemEnumName_itemsData_illustratedBook[choosed];

        if(item.itemType == ItemType.Item)
        {
            bool isUseSuccessed = InventoryManager.Instance.UseItem(choosed);
            if(isUseSuccessed == true)
            {
                //成功觸發效果
                itemEnumName_items_illustratedBook[item.itemEnumName].ItemUsing();
            }
        }
        else if(item.itemType == ItemType.Drop)
        {
            Debug.LogWarning("ItemManager: 掉落物無法被直接使用");
            EventManager.TriggerEvent(NameOfEvent.ShowMessage, $"掉落物無法被直接使用");
        }
    }

    /// <summary>
    /// 根據index 去選擇指定的物品
    /// 跟UI那邊應該是生成UI的時候 要順便在UI那邊儲存index
    /// </summary>
    /// <param name="index">選擇的編號</param>
    public void ItemChoosed(int index)
    {
        if(0 <= index && index < itemEnumName_itemsData_illustratedBook.Count)
        {
            choosed = (ItemName)index;
            Debug.Log($"ItemManager: 選擇道具 {itemEnumName_itemsData_illustratedBook[choosed].itemName}");
        }
        else
        {
            Debug.LogWarning("ItemManager: 選擇不存在的道具");
        }
    }


    /// <summary>
    /// 添加指定編號的物件進入manager
    /// </summary>
    public void AddItem(int index)
    {
        if (0 <= index && index < itemEnumName_itemsData_illustratedBook.Count)
        {
            InventoryManager.Instance.AddItem((ItemName)index);
        }
        else
        {
            Debug.LogWarning("ItemManager: 添加不存在的道具");
        }
    }
}
