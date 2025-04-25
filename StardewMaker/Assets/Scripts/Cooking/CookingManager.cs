using System;
using UnityEngine;

public class CookingManager : Singleton<CookingManager>
{
    public Recipe[] recipes; // 요리 레시피 배열

    public CookingUI cookingUI;

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
        recipes[0].UnlockRecipe(); // 첫 번째 레시피 잠금 해제
        recipes[1].UnlockRecipe(); // 두 번째 레시피 잠금 해제
    }

    private void InitRecipe()
    {
        // 레시피 추가
        for (int i = 0; i < recipes.Length; i++)
        {
            cookingUI.AddRecipe(recipes[i]);
        }
    }

    public void Cook(Recipe currentRecipe)
    {
        // 요리하기 버튼 클릭 시 호출되는 메서드
        for(int i = 0; i < currentRecipe.ingredients.Length; i++)
        {
            // 재료 확인
            if (Inventory.Instance.CheckItem(currentRecipe.ingredients[i]) == false)
            {
                Debug.Log("재료가 부족합니다.");
                return;
            }
        }
        // 재료가 모두 있는 경우 요리 진행
        for (int i = 0; i < currentRecipe.ingredients.Length; i++)
        {
            Inventory.Instance.RemoveItem(currentRecipe.ingredients[i]);
        }
        Inventory.Instance.AddItem(currentRecipe.finishedDish); // 요리 결과 아이템 추가
        // 요리 완료 후 결과 UI 표시
        Debug.Log("요리 완성!");
        cookingUI.cookingInventory.UpdateUI();



    }
}
