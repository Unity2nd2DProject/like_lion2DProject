using UnityEngine;
using UnityEngine.UI;

public class RecipeInfoUI : MonoBehaviour
{
    Image dishIcon; // 완성된 요리 아이콘
    Image[] ingredientIcons; // 재료 아이콘들

    Text dishNameText; // 요리 이름
    Text dishDescriptionText; // 요리 설명
    Text dishEffectText; // 요리 효과


    public void UpdateRecipeInfo(Recipe recipe)
    {
        // 요리 아이콘 업데이트
        dishIcon.sprite = recipe.finishedDish.icon;
        // 재료 아이콘 업데이트
        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            if (i < recipe.ingredients.Length)
            {
                ingredientIcons[i].sprite = recipe.ingredients[i].icon;
            }
            else
            {
                // TODO: 남는 아이콘 비활성화가 아닌 기본 이미지로 변경
                ingredientIcons[i].gameObject.SetActive(false); // 남는 아이콘은 비활성화
            }
        }
        // 요리 이름, 설명, 효과 업데이트
        dishNameText.text = recipe.recipeName;
        dishDescriptionText.text = recipe.recipeDescription;
        dishEffectText.text = recipe.finishedDish.description;
    }

}
