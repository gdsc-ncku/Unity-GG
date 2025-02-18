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

        ItemManager.Instance.ItemChoosed(index);
    }

    /// <summary>
    /// 生成所有道具到背包底下
    /// </summary>
    public void RandomInitItemInPackage()
    {
        for (int i = 0; i < itemEnumName_itemsPrefabs_illustratedBook.Count; i++)
        {
            ItemData data = itemEnumName_itemsData_illustratedBook[(ItemName)i];
            InventoryManager.Instance.AddItem(data.itemEnumName, 1);
        }
    }

    /// <summary>
    /// 生成當前所有的道具到場上 用於開發者DEBUG
    /// </summary>
    public void RandomInstantiateItem()
    {
        int range = instantiateRange;
        for (int i = 0; i < itemEnumName_itemsPrefabs_illustratedBook.Count; i++)
        {
            ItemData data = itemEnumName_itemsData_illustratedBook[(ItemName)i];
            if (data.itemType == ItemType.Drop)
            {
                GameObject obj = Instantiate(itemEnumName_itemsPrefabs_illustratedBook[(ItemName)i], itemInstantiatePoint);
                obj.transform.localPosition = new Vector3(Random.Range(-range, range),
                                                        Random.Range(1, range),
                                                        Random.Range(-range, range));

                //設置特效
                GameObject partical = Instantiate(particalEffect, obj.transform);

                //設置名稱
                GameObject canva = Instantiate(itemCanva, obj.transform);
                canva.transform.localPosition = new Vector3(0, 1f, 0);
                canva.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = obj.GetComponent<Item>().itemData.itemName;

            }
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

        ItemManager.Instance.AddItem(index);
    }
}
