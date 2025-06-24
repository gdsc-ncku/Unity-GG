using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;
using UniRx;
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

    private ItemName choosed = ItemName.None; //當前選中的道具

    [Header("物件掉落相關")]
    public float dropRegion = 2f; //道具丟棄範圍

    [SerializeField] private GameObject particalEffect;
    [SerializeField] private GameObject itemCanva;

    [Header("物件拾取相關")]
    public float pickupRange = 2f; // 拾取範圍
    public LayerMask pickupLayer;  // 只檢測掉落物的 Layer
    public GameObject pickupText; // UI 提示（"按 F 拾取"）

    private Transform mainCamera;
    private GameObject currentItem; // 當前可拾取的物品

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

            //拾取相關初始化
            mainCamera = Camera.main.transform;
            pickupText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Duplicate creating Item Manager Instance");
            Destroy(gameObject);
        }
    }
    #endregion

    void Update()
    {
        PickDetection();
    }

    /// <summary>
    /// 快捷設定道具
    /// </summary>
    /// <param name="index"></param>
    public void SetQuickProp(int index)
    {
        if(choosed == ItemName.None)
        {
            Debug.LogWarning("ItemManager: None choose item");
            return;
        }
        EventManager.TriggerEvent(NameOfEvent.SetQuickProp, index, choosed);

        //處理當前已經選擇的欄位
        choosed = ItemName.None;

        Debug.Log($"ItemManager: set quick item {choosed.ToString()} at {index}");
    }

    /// <summary>
    /// 拾取物品檢測
    /// </summary>
    private void PickDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, pickupRange, pickupLayer))
        {
            // 如果射線擊中掉落物
            if (hit.collider.CompareTag("Item"))
            {
                //清除舊選取物的外框
                if(currentItem != null && currentItem != hit.collider.gameObject)
                    currentItem.GetComponent<Outline>().enabled = false;

                currentItem = hit.collider.gameObject;

                Item item = currentItem.GetComponent<Item>();


                pickupText.SetActive(true); // 顯示 UI 提示
                pickupText.GetComponentInChildren<TextMeshProUGUI>().text = $"按\"F\"拾取 {item.itemData.itemName}";
                item.gameObject.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickupItem(item);
                }
            }
        }
        else
        {
            // 射線沒有碰到可拾取物品
            if(currentItem != null)
                currentItem.GetComponent<Outline>().enabled = false;

            currentItem = null;
            pickupText.SetActive(false); // 隱藏 UI
        }
    }

    /// <summary>
    /// 拾取掉落物
    /// </summary>
    private void PickupItem(Item item)
    {
        Debug.Log("ItemManager: 拾取道具");

        ItemName itemName = item.itemData.itemEnumName;
        Destroy(item.gameObject);

        AddItem(itemName);
    }

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
                choosed = ItemName.None;
            }
        }
        else if(item.itemType == ItemType.Drop)
        {
            Debug.LogWarning("ItemManager: 掉落物無法被直接使用");
            EventManager.TriggerEvent(NameOfEvent.ShowMessage, $"掉落物無法被直接使用");
        }
    }

    /// <summary>
    /// 丟棄道具
    /// </summary>
    private void ItemDrop()
    {
        BoolWrapper isDropSuccess = new BoolWrapper(false);

        //inventory manager deal with item drop...
        EventManager.TriggerEvent(NameOfEvent.DropItem, choosed, isDropSuccess);

        if(isDropSuccess.Value == true)
        {
            //base on item name find object, than ini. it near player

            Debug.Log("ItemManager: 丟棄成功 生成掉落物");

            InstantiateItemObject(choosed);
            choosed = ItemName.None;
        }
    }

    /// <summary>
    /// 生成指定的道具遊戲物件
    /// </summary>
    /// <param name="choosed"></param>
    public void InstantiateItemObject(ItemName choosed)
    {
        GameObject obj = Instantiate(itemEnumName_itemsPrefabs_illustratedBook[choosed], PlayerManager.Instance.transform);
        obj.transform.localPosition = new Vector3(UnityEngine.Random.Range(-dropRegion, dropRegion),
                                                UnityEngine.Random.Range(1, dropRegion),
                                                UnityEngine.Random.Range(-dropRegion, dropRegion));
        obj.transform.SetParent(null);

        //設置特效
        GameObject partical = Instantiate(particalEffect, obj.transform);

        //設置名稱
        GameObject canva = Instantiate(itemCanva, obj.transform);
        canva.transform.localPosition = new Vector3(0, 1f, 0);
        canva.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = obj.GetComponent<Item>().itemData.itemName;

        //設置選取外框
        Outline outline = obj.AddComponent<Outline>();
        outline.OutlineWidth = 5;
        outline.enabled = false;
        outline.OutlineColor = Color.red;
    }

    /// <summary>
    /// 根據index 去選擇指定的物品
    /// 跟UI那邊應該是生成UI的時候 要順便在UI那邊儲存index
    /// </summary>
    /// <param name="itemName">選擇的道具名稱</param>
    public void ItemChoosed(ItemName itemName)
    {
        choosed = itemName;
        Debug.Log($"ItemManager: 選擇道具 {itemEnumName_itemsData_illustratedBook[choosed].itemName}");
    }

    /// <summary>
    /// 添加指定編號的物件進入manager
    /// </summary>
    public void AddItem(ItemName itemName)
    {
        InventoryManager.Instance.AddItem(itemName);
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening(
            NameOfEvent.DropItem_ItemManager,
            () => ItemDrop()
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // 自動取消所有事件訂閱
    }
    #endregion

    // 存取 itemEnumName_itemsPrefabs_illustratedBook 的函式
    public GameObject GetItemPrefab(ItemName itemName)
    {
        if (itemEnumName_itemsPrefabs_illustratedBook.ContainsKey(itemName))
        {
            return itemEnumName_itemsPrefabs_illustratedBook[itemName];
        }
        else
        {
            Debug.LogError($"Item prefab for {itemName} not found.");
            return null;
        }
    }

    // 存取 itemEnumName_itemsData_illustratedBook 的函式
    public ItemData GetItemData(ItemName itemName)
    {
        if (itemEnumName_itemsData_illustratedBook.ContainsKey(itemName))
        {
            return itemEnumName_itemsData_illustratedBook[itemName];
        }
        else
        {
            Debug.LogError($"Item data for {itemName} not found.");
            return null;
        }
    }

    // 存取 itemEnumName_items_illustratedBook 的函式
    public Item GetItemScript(ItemName itemName)
    {
        if (itemEnumName_items_illustratedBook.ContainsKey(itemName))
        {
            return itemEnumName_items_illustratedBook[itemName];
        }
        else
        {
            Debug.LogError($"Item script for {itemName} not found.");
            return null;
        }
    }
}
