using TMPro;
using UniRx;
using UnityEngine;

/// <summary>
/// 道具快捷使用系統
/// 負責代碼方面的道具快捷使用
/// </summary>
public class PropsQuickUseSystem : MonoSingleton<PropsQuickUseSystem>
{
    //快捷欄位
    public ItemName[] quickProps = new ItemName[4] { ItemName.None, ItemName.None, ItemName.None, ItemName.None};
    public TextMeshProUGUI[] probsText = new TextMeshProUGUI[4];

    [Header("快捷施放")]
    private bool isQuickUseMode = false;
    private Vector2 startMousePos;
    [SerializeField]private int selectedIndex = -1; // 當前選中的道具索引
    public float cancelDistance = 100f; // 移動超過這個距離就取消選擇

    [Header("選取特效")]
    public GameObject quickPropsUI;

    public Color selectedColor = Color.yellow; // 選中時的字體顏色
    public Color normalColor = Color.white; // 未選中時的字體顏色

    private int previousSelectedIndex = -2; // 追蹤上一次選中的索引

    private void Start()
    {
        quickPropsUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartQuickUseMode();
        }

        if (isQuickUseMode)
        {
            UpdateSelection();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            EndQuickUseMode();
        }
    }

    /// <summary>
    /// 設置快捷欄位
    /// </summary>
    /// <param name="index"></param>
    /// <param name="itemName"></param>
    private void SetQuickProp(int index, ItemName itemName)
    {
        quickProps[index] = itemName;
    }

    private void StartQuickUseMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isQuickUseMode = true;
        startMousePos = Input.mousePosition; // 記錄按下 F 時的滑鼠位置
        selectedIndex = -1; // 重置選擇

        quickPropsUI.SetActive(true);
        UpdateTextInfo();
    }

    private void UpdateSelection()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 direction = currentMousePos - startMousePos;
        float distance = direction.magnitude;

        if (distance > cancelDistance)
        {
            selectedIndex = -1; // 超出範圍，取消選擇
            UpdateTextEffects(); // 更新字體特效
            previousSelectedIndex = selectedIndex; // 更新上一次選中的索引
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360; // 確保角度是 0~360

        if (distance > 30f) // 滑鼠需要有一定移動量才觸發選擇
        {
            if (angle >= 45 && angle < 135)
                Select(0); // 向上
            else if ( angle >= 135 && angle < 225)
                Select(1); // 向左
            else if (angle >= 225 && angle < 315)
                Select(2); // 向下
            else
                Select(3); // 向右
        }

        // 檢查選中的索引是否改變
        if (selectedIndex != previousSelectedIndex)
        {
            UpdateTextEffects(); // 更新字體特效
            previousSelectedIndex = selectedIndex; // 更新上一次選中的索引
        }
    }

    private void UpdateTextEffects()
    {
        // 重置所有字體顏色為正常顏色
        for (int i = 0; i < probsText.Length; i++)
        {
            if (probsText[i] != null)
            {
                probsText[i].color = normalColor;
            }
        }

        // 選中後改變對應字體顏色
        if (selectedIndex >= 0 && selectedIndex < probsText.Length && probsText[selectedIndex] != null)
        {
            probsText[selectedIndex].color = selectedColor;
        }
    }

    private void Select(int index)
    {
        selectedIndex = index;
        
    }

    private void EndQuickUseMode()
    {
        isQuickUseMode = false;

        if (selectedIndex >= 0 && selectedIndex < quickProps.Length)
        {
            UseItem(quickProps[selectedIndex]); // 使用選中的道具
            UpdateTextInfo();
        }

        selectedIndex = -1; // 重置選擇

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        quickPropsUI.SetActive(false);
    }

    private void UseItem(ItemName item)
    {
        if (item == ItemName.None) return;
        Debug.Log($"使用道具: {item}");
        // 在這裡實作使用道具的邏輯

        ItemManager.Instance.ItemChoosed(item);
        ItemManager.Instance.ItemTrigger();
    }

    /// <summary>
    /// 更新快捷圓圈中的文字
    /// </summary>
    private void UpdateTextInfo()
    {
        for(int i = 0; i < 4; i++)
        {
            //預設
            probsText[i].text = "[]";
            if (quickProps[i] == ItemName.None) {
                continue;
            }

            ItemData itemData = ItemManager.Instance.GetItemData(quickProps[i]);

            //檢查欄位資料是否存在
            if (itemData != null)
            {
                ItemName itemName = itemData.itemEnumName;
                bool isExist = InventoryManager.Instance.SearchItem(itemName);

                //檢查該物品在背包中是否存在
                if (isExist == true) {
                    string info = itemData.itemName;
                    probsText[i].text = info;
                }
                else
                {
                    quickProps[i] = ItemName.None;
                }
            }
        }
    }

    #region event
    private CompositeDisposable disposables = new CompositeDisposable();

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening<int, ItemName>(
            NameOfEvent.SetQuickProp,
            (index, itemName) => SetQuickProp(index, itemName)
        ));
    }

    private void OnDisable()
    {
        disposables.Clear(); // 自動取消所有事件訂閱
    }
    #endregion
}
