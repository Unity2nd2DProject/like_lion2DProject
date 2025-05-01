using System;
using UnityEngine;

public class CookingManager : Singleton<CookingManager>
{
    public Recipe[] recipes; // 요리 레시피 배열 수동으로 inspector에서 추가 요망.

    protected override void Awake()
    {
        base.Awake();

        
    }

    private void Start()
    {
        InitRecipe();
        InitTest();
    }
    // 테스트용 레시피 잠금 해제
    private void InitTest()
    {
        recipes[0].UnlockRecipe(); 
        recipes[1].UnlockRecipe(); 
        recipes[2].UnlockRecipe(); 
        recipes[3].UnlockRecipe(); 
        recipes[4].UnlockRecipe(); 
        recipes[5].UnlockRecipe();        
    }

    private void InitRecipe()
    {
        UIManager.Instance.InitializeCookingUI();
        // 레시피 추가
        for (int i = 0; i < recipes.Length; i++)
        {
            UIManager.Instance.cookingUI.AddRecipe(recipes[i]);
        }
    }

    public void Cook(Recipe currentRecipe)
    {
        // 요리하기 버튼 클릭 시 호출되는 메서드
        for(int i = 0; i < currentRecipe.ingredients.Length; i++)
        {
            // 재료 확인
            if (InventoryManager.Instance.CheckItem(currentRecipe.ingredients[i]) == false)
            {
                UIManager.Instance.ShowPopup("재료가 부족합니다.");
                return;
            }
        }
        // 재료가 모두 있는 경우 요리 진행
        for (int i = 0; i < currentRecipe.ingredients.Length; i++)
        {
            InventoryManager.Instance.RemoveItem(currentRecipe.ingredients[i]);
        }
        InventoryManager.Instance.AddItem(currentRecipe.finishedDish); // 요리 결과 아이템 추가
        // 요리 완료 후 결과 UI 표시
        UIManager.Instance.ShowPopup("요리 완성!.");
        UIManager.Instance.cookingUI.cookingInventory.UpdateIngredientInventoryUI();

    }
}
