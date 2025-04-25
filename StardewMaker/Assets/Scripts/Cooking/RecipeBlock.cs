using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBlock : MonoBehaviour
{
    private Recipe recipe; // 레시피 데이터

    public Image dishIcon;
    public TextMeshProUGUI dishName;
    public Button recipeButton; // 레시피 블록 버튼

    internal void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        dishIcon.sprite = recipe.finishedDish.icon;
        dishName.text = recipe.recipeName;
    }

    private void Start()
    {
        recipeButton.onClick.AddListener(OnRecipeClicked);
    }

    private void OnRecipeClicked()
    {
        CookingUI.Instance.ShowRecipeInfo(recipe);
    }
}
