using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 用於測試道具UI開發 開發者的腳本
/// </summary>
public class UIDevelopeTester : MonoBehaviour
{
    [Header("For develop")]
    //開發使用區
    [SerializeField] private ItemName wantAddItem;
    [SerializeField] private TMP_InputField wantedAddIndex;
    [SerializeField] private TMP_InputField inputChooseIndex;
    [SerializeField] private Transform itemInstantiatePoint;    //物件生成位置
    [SerializeField] private int instantiateRange = 5;
    [SerializeField] private GameObject particalEffect;
    [SerializeField] private GameObject itemCanva;

    [HideInInspector] public Dictionary<ItemName, GameObject> itemEnumName_itemsPrefabs_illustratedBook = new Dictionary<ItemName, GameObject>(); //所有的item資訊
    [HideInInspector] public Dictionary<ItemName, ItemData> itemEnumName_itemsData_illustratedBook = new Dictionary<ItemName, ItemData>(); //所有的item資訊

    private void Start()
    {
        itemEnumName_itemsPrefabs_illustratedBook = ItemManager.Instance.itemEnumName_itemsPrefabs_illustratedBook;
        itemEnumName_itemsData_illustratedBook = ItemManager.Instance.itemEnumName_itemsData_illustratedBook;
    }

    /// <summary>
    /// 觸發道具
    /// </summary>
    public void ItemTrigger()
    {
        ItemManager.Instance.ItemTrigger();
    }

    /// <summary>
    /// 開發者用的多載 載入指定輸入框的index
    /// </summary>
    /// <param name="index">選擇的編號</param>
    public void ItemChoosed()
    {
        //如果index 是-1 代表是由開發者藉由測試觸發 載入測試面板的資訊
        int index = int.Parse(inputChooseIndex.text);
        ItemName itemName = (ItemName)index;

        ItemManager.Instance.ItemChoosed(itemName);
    }

    /// <summary>
    /// 生成所有道具到背包底下
    /// </summary>
    public void RandomInitItemInPackage()
    {
        foreach(var dataPair in ItemManager.Instance.itemEnumName_itemsData_illustratedBook)
        {
            ItemData data = dataPair.Value;
            if (data != null)
            {
                InventoryManager.Instance.AddItem(data.itemEnumName, 1);
            }
        }

        //更新道具顯示介面
        EventManager.TriggerEvent(NameOfEvent.UpdateItem);
    }

    /// <summary>
    /// 生成當前所有的道具到場上 用於開發者DEBUG
    /// </summary>
    public void RandomInstantiateItem()
    {
        int range = instantiateRange;
        foreach (var dataPair in ItemManager.Instance.itemEnumName_itemsData_illustratedBook)
        {
            ItemName itemName = dataPair.Key;
            ItemManager.Instance.InstantiateItemObject(itemName);
        }
    }

    /// <summary>
    /// 測試用函數
    /// 添加一個物件進入manager
    /// </summary>
    public void AddItem()
    {
        //GameObject obj = itemEnumName_items_illustratedBook[wantAddItem];
        //GameObject item = Instantiate(obj, this.transform);

        //package.Add(item);

        int index = int.Parse(wantedAddIndex.text);
        ItemName itemName = (ItemName)index;
        ItemManager.Instance.AddItem(itemName);
    }
}
