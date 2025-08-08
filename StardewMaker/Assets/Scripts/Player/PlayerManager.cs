using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("traits")]
    [SerializeField] private List<TraitInstance> traits;

    [Header("stats")]
    public float cropGrowthSpeedBonus = 0f;
    public float foodEffectivenessBonus = 0f;
    public float fishingSuccessWindowBonus = 0f;
    public float rareFishChanceBonus = 0f;
    public float criticalChanceBonus = 0f;
    public float pickSpeedBonus = 0f;
    public float moveSpeedBonus = 0f;

    public float daughterRecoveryBoost = 0f;
    public float daughterStatGainChance = 0f;

    public bool canPredictDaughterEmotion = false;

    public List<int> unlockedEvents = new List<int>();
    public List<string> unlockedItems = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        InitTraits();
    }

    private void InitTraits()
    {
        traits = new List<TraitInstance>();

        foreach (var traitData in TraitManager.Instance.allTraits)
        {
            TraitInstance newInstance = new TraitInstance(traitData.traitId);
            traits.Add(newInstance);
        }
    }

    public void AddExpToSkill(TraitType traitType, int amount = 1)
    {
        foreach (var trait in traits)
        {
            TraitData data = TraitManager.Instance.GetTraitById(trait.traitId);
            if (data != null && data.skillType == traitType)
            {
                bool leveledUp = trait.AddExp(amount);
                Debug.Log($"[PlayerManager] {data.traitName} 경험치 획득! (Level: {trait.currentLevel}, EXP: {trait.currentExp}(+{amount}))");

                if (leveledUp)
                {
                    Debug.Log($"[PlayerManager] {data.traitName} 레벨업!  (Level: {trait.currentLevel}");
                    UpdatePlayerStats();
                }
            }
        }
    }

    private void UpdatePlayerStats()
    {
        foreach (var trait in traits)
        {
            TraitData data = TraitManager.Instance.GetTraitById(trait.traitId);
            if (data == null)
            {
                continue;
            }

            if (trait.currentLevel < 5)
            {
                continue;
            }

            trait.ApplyToPlayerStats();
        }
    }

    public TraitInstance GetTraitInstance(int traitId)
    {
        return traits.Find(t => t.traitId == traitId);
    }
}
