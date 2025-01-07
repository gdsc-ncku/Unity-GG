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
    /// 嘗試合成
    /// </summary>
    /// <param name="inputMaterials">輸入的合成材料</param>
    /// <returns>合成結果</returns>
    public ItemData TryCraft(List<ItemData> inputMaterials)
    {
        foreach (var recipe in recipeBook.recipes)
        {
            // 檢查材料是否符合配方
            if (IsRecipeMatch(recipe, inputMaterials))
            {
                return recipe.resultItem;
            }
        }
        return null; // 沒有符合的配方
    }

    /// <summary>
    /// 檢查是否符合組合圖的邏輯
    /// </summary>
    /// <param name="recipe">組合圖</param>
    /// <param name="inputMaterials">輸入材料</param>
    /// <returns></returns>
    private bool IsRecipeMatch(Recipe recipe, List<ItemData> inputMaterials)
    {
        // 簡單匹配邏輯，檢查數量與名稱是否一致
        foreach (var material in recipe.requiredMaterials)
        {
            if (!inputMaterials.Contains(material))
                return false;
        }
        return true;
    }
}
