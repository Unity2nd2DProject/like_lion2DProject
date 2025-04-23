using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "recipe1", menuName = "Recipe / Create New Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public Sprite icon;
    public string recipeDescription;

    public ItemData finishedDish; // 완성품
    public ItemData[] ingredients;

    public bool isUnlocked; // 레시피 잠금 해제 여부
    public void UnlockRecipe()
    {
        isUnlocked = true;
    }
}
