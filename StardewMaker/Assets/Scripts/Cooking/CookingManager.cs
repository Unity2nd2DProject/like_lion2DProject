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

    }

    private void InitRecipe()
    {
        // 레시피 추가
        for (int i = 0; i < recipes.Length; i++)
        {
            cookingUI.AddRecipe(recipes[i]);
        }
    }

}
