using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成圖的總和 一本合成書
/// </summary>
[CreateAssetMenu(fileName = "NewRecipeBook", menuName = "Crafting/RecipeBook")]
[System.Serializable]
public class RecipeBook : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
}
