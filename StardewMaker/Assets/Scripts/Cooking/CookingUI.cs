using System;
using UnityEngine;

public class CookingUI : Singleton<CookingUI>
{

    public GameObject recipeListParent; // content
    public GameObject recipeBlockPrefab;


    public RecipeInfoUI recipeInfoUI; // 레시피 정보 UI
    public CookingInventory cookingInventory; // 요리 인벤토리 UI

    protected override void Awake()
    {
        base.Awake();
    }   

    public void AddRecipe(Recipe recipe)
    {
        var recipeListBlock = Instantiate(recipeBlockPrefab, recipeListParent.transform);
        recipeListBlock.GetComponent<RecipeBlock>().SetRecipe(recipe);
    }

    public void ShowRecipeInfo(Recipe recipe)
    {
        recipeInfoUI.UpdateRecipeInfo(recipe);
    }


    // 1. 리스트 생성, INSTATIATE
    // 2. 리스트 생성된 버튼에 onclicick 에 함수 추가...? 

    // 그냥 해당 정보 올리면 되지 않나




    // 현재 선택한거 고르면 

    // 조건이 된다면? <- 이거 판정도 쿠킹매니저

    // cookingmanager 에서 만들기
    // -> 재료 삭제 / 요리 추가 


    // 조건이 안된다면 -> 여기서 뭔가 해줘야지. 
}
