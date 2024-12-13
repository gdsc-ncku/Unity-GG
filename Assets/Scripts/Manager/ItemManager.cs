using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    #region 用於建立物件資料 對於 物件腳本的圖鑑
    public List<GameObject> itemPrefabs = new List<GameObject>();
    [HideInInspector] public Dictionary<ItemData, GameObject> itemData_items_illustratedBook = new Dictionary<ItemData, GameObject>(); //所有的item資訊
    [HideInInspector] public Dictionary<ItemName, GameObject> itemEnumName_items_illustratedBook = new Dictionary<ItemName, GameObject>(); //所有的item資訊
    #endregion


    public List<GameObject> package = new List<GameObject>();  //用來存放生成的道具物件
    private Item choosedItem;  //目前選擇的物件

    [Header("For develop")]
    //開發使用區
    [SerializeField] private ItemName wantAddItem;
    [SerializeField] private TMP_InputField inputChooseIndex;

    #region 建立單例模式
    //instance mode
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find Craft Manager Instance");
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
                GameObject obj = itemPrefabs[i];
                Item item = itemPrefabs[i].GetComponent<Item>();
                ItemData data = item.itemData;
                ItemName itemEnumName = data.itemEnumName;

                itemData_items_illustratedBook[data] = obj;
                itemEnumName_items_illustratedBook[itemEnumName] = obj;
            }
        }
        else
        {
            Debug.LogError("Duplicate creating Craft Manager Instance");
            Destroy(gameObject);
        }
    }
    #endregion

    public void ItemTrigger()
    {
        if (choosedItem != null)
        {
            choosedItem.ItemUsing();
        }
        else
        {
            Debug.LogWarning("Item Manager: 觸發道具 但未選擇道具");
        }
    }

    /// <summary>
    /// 根據index 去選擇指定的物品
    /// 跟UI那邊應該是生成UI的時候 要順便在UI那邊儲存index
    /// </summary>
    /// <param name="index">選擇的編號</param>
    public void ItemChoosed(int index)
    {
        if (index == -1) index = int.Parse(inputChooseIndex.text);

        //留給之後可能需要從外部接入 擁有的道具
        List<GameObject> items = package;

        if (items.Count > index && items[index] != null)
        {
            choosedItem = items[index].GetComponent<Item>();
        }
        else
        {
            Debug.LogWarning("Item Manager: 選擇不存在的道具");
        }
    }

    #region 開發區域
    /// <summary>
    /// 測試用函數
    /// 添加一個物件進入manager
    /// </summary>
    public void AddItem()
    {
        GameObject obj = itemEnumName_items_illustratedBook[wantAddItem];
        GameObject item = Instantiate(obj, this.transform);

        package.Add(item);
    }
    #endregion
}
