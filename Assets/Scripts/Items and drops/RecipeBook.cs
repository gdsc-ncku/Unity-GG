using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X���Ϫ��`�M �@���X����
/// </summary>
[CreateAssetMenu(fileName = "NewRecipeBook", menuName = "Crafting/RecipeBook")]
[System.Serializable]
public class RecipeBook : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
}
