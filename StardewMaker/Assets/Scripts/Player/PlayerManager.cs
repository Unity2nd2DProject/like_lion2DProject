using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Info")]
    [SerializeField] private List<TraitInstance> traits;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddExpToSkill(TraitType traitType, float amount)
    {
        foreach (var trait in traits)
        {
            TraitData data = TraitManager.Instance.GetTraitById(trait.traitId);
            if (data != null && data.skillType == traitType)
            {
                bool leveledUp = trait.AddExp(amount);
                Debug.Log($"Added {amount} EXP to {data.traitName}. Level: {trait.currentLevel}, EXP: {trait.currentExp}");

                if (leveledUp)
                {
                    Debug.Log($"Trait '{data.traitName}' leveled up to {trait.currentLevel}!");

                }
            }
        }
    }

    public TraitInstance GetTraitInstance(int traitId)
    {
        return traits.Find(t => t.traitId == traitId);
    }

    public void ApplyTraitEffect()
    {

    }
}
