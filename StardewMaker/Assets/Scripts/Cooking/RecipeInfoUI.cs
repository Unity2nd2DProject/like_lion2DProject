using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeInfoUI : MonoBehaviour
{
    public Image dishIcon; // 완성된 요리 아이콘
    public Image[] ingredientIcons; // 재료 아이콘들

    public TextMeshProUGUI dishNameText; // 요리 이름
    public TextMeshProUGUI dishDescriptionText; // 요리 설명
    public TextMeshProUGUI dishEffectText; // 요리 효과

    public Button cookButton; // 요리하기 버튼

    private Recipe currentRecipe; // 현재 선택된 레시피

    private void Start()
    {
        cookButton.onClick.AddListener(OnCookButtonClicked); // 요리하기 버튼 클릭 이벤트 등록
    }

    public void UpdateRecipeInfo(Recipe recipe)
    {
        currentRecipe = recipe; // 현재 레시피 저장
        // 요리 아이콘 업데이트
        dishIcon.sprite = recipe.finishedDish.icon;
        // 재료 아이콘 업데이트
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            ingredientIcons[i].sprite = null; // 재료 아이콘 초기화
        }
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            if (i < recipe.ingredients.Length)
            {
                ingredientIcons[i].sprite = recipe.ingredients[i].icon;
            }
        }
        // 요리 이름, 설명, 효과 업데이트
        dishNameText.text = recipe.recipeName;
        dishDescriptionText.text = recipe.recipeDescription;

    }

    void OnCookButtonClicked()
    {
        CookingManager.Instance.Cook(currentRecipe); // 요리하기 버튼 클릭 시 CookingManager에 요리 요청
    }

}
