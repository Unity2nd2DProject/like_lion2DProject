using UnityEngine;

public class TraitInstance
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

    public bool ApplyToPlayerStats()
    {
        TraitData traitData = TraitManager.Instance.GetTraitById(traitId);

        if (traitData == null || traitData.effects == null)
        {
            return false;
        }

        UpdateStats(traitData);
        return false;
    }

    private void UpdateStats(TraitData traitData)
    {
        foreach (var effect in traitData.effects)
        {
            switch (effect.effectType)
            {
                case TraitEffectType.StatModifier:
                    UpdateStatEffect(effect);
                    break;
                case TraitEffectType.EventUnlock:
                    break;
                case TraitEffectType.ItemUnlock:
                    break;
                case TraitEffectType.RecipeUnlock:
                    break;
                case TraitEffectType.InteractionBoost:
                    break;
                case TraitEffectType.CutsceneUnlock:
                    break;
            }
        }
    }

    private void UpdateStatEffect(TraitEffectData effect)
    {
        float ratio = GetTraitEffectRatio();
        float value = effect.effectValue * ratio;

        switch (effect.statTarget)
        {
            case StatEffectTarget.CropGrowthSpeed:
                PlayerManager.Instance.cropGrowthSpeedBonus = value;
                break;
            case StatEffectTarget.FoodEffectiveness:
                PlayerManager.Instance.foodEffectivenessBonus = value;
                break;
            case StatEffectTarget.FishingSuccessWindow:
                break;
            case StatEffectTarget.RareFishChance:
                break;
            case StatEffectTarget.CriticalChance:
                break;
            case StatEffectTarget.PickSpeed:
                break;
            case StatEffectTarget.MoveSpeed:
                break;
            case StatEffectTarget.DaughterRecovery:
                break;
            case StatEffectTarget.DaughterStatGainChance:
                break;
        }
    }

    private float GetTraitEffectRatio()
    {
        if (currentLevel < 5)
        {
            return 0f;
        }
        else if (currentLevel < 10) // 1/8
        {
            return 0.125f;
        }
        else if (currentLevel < 15) // 1/4
        {
            return 0.25f;
        }
        else if (currentLevel < 20) // 1/2
        {
            return 0.5f;
        }
        else // full effect at max level
        {
            return 1f;
        }
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
