using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    #region 建立單例模式
    //instance mode
    private static RecipeBook _instance;
    public static RecipeBook Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Can't Find RecipeBook Instance");
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
            Debug.LogError("Duplicate creating RecipeBook Instance");
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject recipeUIPrefab; // 單個配方的 UI 項目
    public Transform contentPanel;   // 用於顯示配方的容器

    /// <summary>
    /// 使用craft manager的合成圖
    /// </summary>
    public void UpdateRecipeBook()
    {
        List<Recipe> recipes = CraftManager.Instance.recipes;

        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject); // 清除舊的 UI 項目
        }

        foreach (var recipe in recipes)
        {
            var uiItem = Instantiate(recipeUIPrefab, contentPanel);
            var uiText = uiItem.GetComponentInChildren<TextMeshProUGUI>();
            //var uiImage = uiItem.GetComponentInChildren<Image>();

            if (recipe != null && recipe.isUnlocked)
            {
                string required = "";
                for (int i = 0; i < recipe.requiredMaterials.Count; i++)
                {
                    if (i != 0) required += ", ";

                    required += recipe.requiredMaterials[i].itemName;
                    //required += recipe.requiredMaterials[i].itemEnumName.ToString();
                }
                uiText.text = $"[{required}] → {recipe.resultItem.itemName}";
                //uiImage.sprite = recipe.resultItem.itemSprite;
            }
            else
            {
                uiText.text = "??? → ???";
                //uiImage.sprite = null; // 顯示問號圖示
            }
        }
    }

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
                string required = "";
                for(int i=0;i<recipe.requiredMaterials.Count;i++)
                {
                    if(i != 0) required += ", ";
                    //required += recipe.requiredMaterials[i].itemName;
                    required += recipe.requiredMaterials[i].itemEnumName.ToString();
                }
                uiText.text = $"[{required}] → {recipe.resultItem.itemName}";
                //uiImage.sprite = recipe.resultItem.itemSprite;
            }
            else
            {
                uiText.text = "??? → ???";
                //uiImage.sprite = null; // 顯示問號圖示
            }
        }
    }
}
