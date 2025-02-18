using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UniRx;
using System;
using System.Security.Cryptography;
using NUnit.Framework.Interfaces;

/// <summary>
/// 負責道具背包的UI控制
/// </summary>
public class ItemUIController : MonoBehaviour
{
    [Header("Overview")]
    public Image itemImage;
    public TextMeshProUGUI itemText;

    [Header("Gadgets")]
    public GameObject gadgetParent;
    public List<GameObject> gadgets = new List<GameObject>();

    [Header("Resources")]
    public GameObject resourceParent;
    public List<GameObject> resources = new List<GameObject>();

    [Header("Craftables")]
    public GameObject craftablesParent;
    public List<GameObject> craftables= new List<GameObject>();

    //資料來源
    [HideInInspector] public Dictionary<ItemName, ItemData> itemEnumName_itemsData_illustratedBook = new Dictionary<ItemName, ItemData>(); //所有的item資訊

    private void Start()
    {
        //抓取當前有幾個可顯示位置
        GetEachObject(gadgetParent, gadgets);
        GetEachObject(resourceParent, resources);
        GetEachObject(craftablesParent, craftables);

        //設定道具資料來源
        itemEnumName_itemsData_illustratedBook = ItemManager.Instance.itemEnumName_itemsData_illustratedBook;
    }

    /// <summary>
    /// 暫時性的把按鍵功能綁到這裡
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            ItemTrigger();
        }
    }

    /// <summary>
    /// 觸發道具
    /// </summary>
    public void ItemTrigger()
    {
        ItemManager.Instance.ItemTrigger();
        UpdateItem();
    }

    /// <summary>
    /// UI顯示更換為指定的物件
    /// </summary>
    public void ItemChoosed(int index)
    {
        if (0 <= index && index < itemEnumName_itemsData_illustratedBook.Count)
        {
            ItemName choosed = (ItemName)index;
            ItemData itemData = itemEnumName_itemsData_illustratedBook[choosed];

            itemImage.sprite = itemData.itemSprite;
            itemText.text = $"道具名稱: {itemData.itemName}\n" +
                $"道具描述: {itemData.itemDescription}\n";

            Debug.Log($"ItemUIController: 選擇道具 {itemData.itemName}");
        }
        else
        {
            Debug.LogWarning("ItemUIController: 選擇不存在的道具");
        }
    }

    /// <summary>
    /// 更新item在背包的顯示資訊 圖片
    /// </summary>
    public void UpdateItem() {
        //顯示之前 先清空舊資料
        CleanAllUIData();

        Dictionary<ItemData, int> inventory = InventoryManager.Instance.inventory;

        int gadgetIndex, craftableIndex;
        gadgetIndex = craftableIndex = 0;

        foreach (var entry in inventory)
        {
            Debug.Log($"- {entry.Key.itemName}: {entry.Value}");
            ItemData data = entry.Key;
            int num = entry.Value;
            
            if(data.itemType == ItemType.Drop)
            {
                craftableIndex = InitEachItemUI(craftableIndex, data, num, craftables);
            }
            else if(data.itemType == ItemType.Item)
            {
                gadgetIndex = InitEachItemUI(gadgetIndex, data, num, gadgets);
            }
        }


    }

    /// <summary>
    /// 透過設定的父物件 自動抓取底下有幾個道具欄位
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="objs"></param>
    private void GetEachObject(GameObject parent, List<GameObject> objs)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                objs.Add(parent.transform.GetChild(i).gameObject);
            }
        }

    /// <summary>
    /// 清空所有的舊資訊
    /// </summary>
    private void CleanAllUIData()
    {
        CleanEachUI(gadgets);
        CleanEachUI(craftables);
        CleanEachUI(resources);

        itemImage.sprite = null;
        itemText.text = $"道具名稱: \n" +
            $"道具描述: \n";
    }

    /// <summary>
    /// 清空單種類別的舊資訊
    /// </summary>
    /// <param name="objs"></param>
    private void CleanEachUI(List<GameObject> objs)
    {
        foreach (var obj in objs)
        {
            //圖片
            Image img = obj.transform.GetChild(0).GetComponent<Image>();
            img.sprite = null;

            //數量
            TextMeshProUGUI text = obj.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            text.text = "x";

            //按鍵
            if(obj.transform.childCount >= 3)
            {
                ItemBtn btn = obj.transform.GetChild(2).GetComponentInChildren<ItemBtn>();
                btn.isSetting = false;
            }
        }
    }

    /// <summary>
    /// 初始化各個道具物件的UI資訊
    /// </summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    /// <param name="num"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    private int InitEachItemUI(int index, ItemData data, int num, List<GameObject> objs)
    {
        //超過顯示上限
        if (index >= objs.Count) return index;

        //圖片
        Image img = objs[index].transform.GetChild(0).GetComponent<Image>();
        img.sprite = data.itemSprite;

        //數量
        TextMeshProUGUI text = objs[index].transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        text.text = num.ToString();

        //按鍵
        ItemBtn btn = objs[index].transform.GetChild(2).GetComponentInChildren<ItemBtn>();
        btn.nameIndex = data.itemEnumName;
        btn.isSetting = true;

        index++;
        return index;
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<int>(
            NameOfEvent.ItemChoosed,
            (index) => ItemChoosed(index)
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // 自動取消所有事件訂閱
    }
    #endregion
}
