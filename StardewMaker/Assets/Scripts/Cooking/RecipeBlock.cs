using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBlock : MonoBehaviour
{
    CookingUI cookingUI; // 레시피 UI 매니저

    private Recipe recipe; // 레시피 데이터

    public Image dishIcon;
    public TextMeshProUGUI dishName;
    public Button recipeButton; // 레시피 블록 버튼

    

    public void InitializeRecipeBlockUI(CookingUI cookingUI)
    {
        this.cookingUI = cookingUI; // 레시피 UI 매니저 초기화

    }
    public void SetRecipe(Recipe recipe)
    {
        if(recipe.isUnlocked == false) return; // 레시피가 잠금 해제되지 않은 경우

        this.recipe = recipe;
        dishIcon.sprite = recipe.finishedDish.icon;
        dishName.text = recipe.recipeName;

        recipeButton.onClick.AddListener(OnRecipeClicked);
    }

    private void OnRecipeClicked()
    {
        cookingUI.ShowRecipeInfo(recipe);
    }
}
