using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 合成管理器
/// 負責處理合成相關的事物
/// </summary>
public class CraftManager : MonoBehaviour
{
    #region 建立單例模式
    //instance mode
    private static CraftManager _instance;
    public static CraftManager Instance
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
        }
        else
        {
            Debug.LogError("Duplicate creating Craft Manager Instance");
            Destroy(gameObject);
        }
    }
    #endregion

    //遊戲中的合成書
    public RecipeBook recipeBook;

    /// <summary>
    /// 嘗試合成指定道具
    /// </summary>
    /// <param name="item">希望合成出來的道具</param>
    /// <returns>合成結果</returns>
    public ItemData TryCraft(ItemName item)
    {
        Recipe targetRecipe = null;

        foreach (var recipe in recipeBook.recipes)
        {
            Debug.Log($"CraftManager: 查找合成圖 {item} : {recipe.resultItem.itemEnumName}");
            if (recipe.resultItem.itemEnumName == item)
            {
                targetRecipe = recipe;
                break;
            }
            
        }

        if (targetRecipe == null)
        {
            Debug.LogWarning($"CraftManager: 找不到對應道具的合成圖 {item}");
            return null;
        }

        if (IsRecipeMatch(targetRecipe) == true)
        {
            ConsumeItem(targetRecipe);
            InventoryManager.Instance.AddItem(targetRecipe.resultItem.itemEnumName, 1);
            Debug.Log($"CraftManager: 合成成功 {item}");
            return targetRecipe.resultItem;
        }
        else
        {
            Debug.LogWarning($"CraftManager: 合成失敗 {item}");
            return null; // 沒有符合的配方
        }

    }

    /// <summary>
    /// 將合成圖的材料消耗掉
    /// </summary>
    /// <param name="recipe"></param>
    private void ConsumeItem(Recipe recipe)
    {
        // 簡單匹配邏輯，檢查數量與名稱是否一致
        foreach (var material in recipe.requiredMaterials)
        {
            InventoryManager.Instance.ConsumeItem(material.itemEnumName, 1);
        }
    }

    /// <summary>
    /// 檢查是否符合組合圖的邏輯
    /// </summary>
    /// <param name="recipe">組合圖</param>
    /// <param name="inputMaterials">輸入材料</param>
    /// <returns></returns>
    private bool IsRecipeMatch(Recipe recipe)
    {
        bool isMatched = true;

        // 簡單匹配邏輯，檢查數量與名稱是否一致
        foreach (var material in recipe.requiredMaterials)
        {
            if (InventoryManager.Instance.SearchItem(material.itemEnumName, 1) == false)
            {
                isMatched = false;
                break;
            } 
        }

        return isMatched;
    }
}
