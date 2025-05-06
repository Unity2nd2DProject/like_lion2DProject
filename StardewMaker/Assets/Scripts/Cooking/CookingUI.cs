using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingUI : MonoBehaviour
{

    public GameObject recipeListParent; // 레시피 리스트의 부모 오브젝트
    public GameObject recipeBlockPrefab; // 레시피 블록 프리팹

    public RecipeInfoUI recipeInfoUI; // 레시피 정보 UI
    public IngredientInventory cookingInventory; // 요리 인벤토리 UI
    public Button exitButton; // 요리 창 닫기 버튼

    void Start()
    {
        exitButton.onClick.AddListener(CloseUI);
    }

    void CloseUI()
    {
        UIManager.Instance.CloseCookingUI();
    }

    public void AddRecipe(Recipe recipe)
    {
        var recipeListBlock = Instantiate(recipeBlockPrefab, recipeListParent.transform);
        recipeListBlock.GetComponent<RecipeBlock>().InitializeRecipeBlockUI(this); // 레시피 블록 초기화
        recipeListBlock.GetComponent<RecipeBlock>().SetRecipe(recipe);
    }

    public void ShowRecipeInfo(Recipe recipe)
    {
        recipeInfoUI.UpdateRecipeInfo(recipe);
    }
}
