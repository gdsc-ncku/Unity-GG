using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 用於在遊戲中顯示合成圖的UGUI腳本
/// 測試用腳本
/// </summary>
public class RecipeBookUGUI : MonoBehaviour
{

    public GameObject recipeUIPrefab; // 單個配方的 UI 項目
    public Transform contentPanel;   // 用於顯示配方的容器

    public GameObject itemUIPrefab; // 單個配方的 UI 項目
    public Transform inventoryPanel;

    [Header("Develop")]
    public GameObject developCanvas;

    //用於事件訂閱
    private CompositeDisposable disposables = new CompositeDisposable();

    public void Update()
    {
        ShowDevelopWindow();
    }   

    /// <summary>
    /// 使用craft manager的合成圖
    /// </summary>
    public void UpdateRecipeBook()
    {
        List<Recipe> recipes = CraftManager.Instance.recipeBook.recipes;

        UpdateRecipeBook(recipes);
    }

    /// <summary>
    /// 根據傳入的合成圖 更新UGUI
    /// </summary>
    /// <param name="recipes"></param>
    public void UpdateRecipeBook(List<Recipe> recipes)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject); // 清除舊的 UI 項目
        }

        foreach (var recipe in recipes)
        {
            var uiItem = Instantiate(recipeUIPrefab, contentPanel);
            var uiText = uiItem.GetComponentInChildren<TextMeshProUGUI>();
            //var uiImage = uiItem.GetComponentInChildren<Image>();

            if (recipe.isUnlocked)
            {
                //顯示合成圖資訊
                string required = "";
                for(int i=0;i<recipe.requiredMaterials.Count;i++)
                {
                    if(i != 0) required += ", ";
                    //required += recipe.requiredMaterials[i].itemName;
                    required += recipe.requiredMaterials[i].itemName.ToString();
                    required += "(" + ((int)recipe.requiredMaterials[i].itemEnumName).ToString() + ")";
                }
                uiText.text = $"[{required}] → {recipe.resultItem.itemName}";
                //uiImage.sprite = recipe.resultItem.itemSprite;

                //紀錄當前合成圖的結果產物
                uiItem.GetComponent<CraftButton>().itemData = recipe.resultItem;
            }
            else
            {
                uiText.text = "??? → ???";
                //uiImage.sprite = null; // 顯示問號圖示
            }
        }
    }

    /// <summary>
    /// 更新背包UI
    /// 顯示當前背包中有甚麼東西
    /// </summary>
    public void UpdateInventory()
    {
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject); // 清除舊的 UI 項目
        }

        foreach (var item in InventoryManager.Instance.inventory)
        {
            var uiItem = Instantiate(itemUIPrefab, inventoryPanel);
            var uiText = uiItem.GetComponentInChildren<TextMeshProUGUI>();
            //var uiImage = uiItem.GetComponentInChildren<Image>();

            uiText.text = $"[{item.Key.itemName}] (編號:{(int)item.Key.itemEnumName}): {item.Value}";
            //uiImage.sprite = recipe.resultItem.itemSprite;
        }
    }

    /// <summary>
    /// 偵測是否需要打開或關上 開發者窗口
    /// </summary>
    private void ShowDevelopWindow()
    {
        if (developCanvas.activeSelf == true && Input.GetKeyDown(KeyCode.O))
        {
            developCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (developCanvas.activeSelf == false && Input.GetKeyDown(KeyCode.O))
        {
            developCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnEnable()
    {
        // 註冊對  事件的訂閱
        disposables.Add(EventManager.StartListening(
            NameOfEvent.InventoryItemChange,
            () => UpdateInventory()
        ));
    }

    private void OnDisable()
    {
        // 取消註冊對  事件的訂閱
        disposables.Clear();
    }

}
