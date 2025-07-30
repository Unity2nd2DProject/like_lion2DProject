using UnityEngine;

public class TraitInstance : MonoBehaviour
{
    public int traitId;
    public float currentExp;
    public int currentLevel;

    public TraitInstance(int id)
    {
        traitId = id;
        currentExp = 0f;
        currentLevel = 1;
    }

    public float GetExpToLevelUp()
    {
        TraitData traitData = TraitManager.Instance.GetTraitById(traitId);
        return traitData.baseExpToLevelUp * Mathf.Pow(traitData.expMultiplier, currentLevel - 1);
    }

    public float GetTraitEffectRatio()
    {
        TraitData traitData = TraitManager.Instance.GetTraitById(traitId);

        if (traitData == null || traitData.effects == null)
        {
            return 1f;
        }

        foreach (var effect in traitData.effects)
        {

        }
        return 1f;
       
        //if (level < 5)
        //{
        //    return 0f;
        //}
        //else if (level < 10)
        //{
        //    return traitData.maxEffectValue * 0.125f; // 1/8
        //}
        //else if (level < 15)
        //{
        //    return traitData.maxEffectValue * 0.25f;  // 1/4
        //}
        //else if (level < 20)
        //{
        //    return traitData.maxEffectValue * 0.5f;   // 1/2
        //}
        //else
        //{
        //    return traitData.maxEffectValue;          // 1.0
        //}
    }

    public bool AddExp(float amount)
    {
        bool isLeveldUp = false;
        TraitData traitData = TraitManager.Instance.GetTraitById(traitId);
        if (currentLevel >= traitData.maxLevel)
        {
            return isLeveldUp;
        }

        currentExp += amount;

        while (currentExp >= GetExpToLevelUp() && currentLevel < traitData.maxLevel)
        {
            isLeveldUp = true;
            currentExp -= GetExpToLevelUp();
            currentLevel++;
            Debug.Log($"[{traitData.traitName}] 레벨업! 현재 레벨: {currentLevel}");
        }

        currentExp = Mathf.Clamp(currentExp, 0f, GetExpToLevelUp());
        return isLeveldUp;
    }
}
