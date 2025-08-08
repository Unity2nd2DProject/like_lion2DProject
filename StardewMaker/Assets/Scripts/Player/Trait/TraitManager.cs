using System.Collections.Generic;
using UnityEngine;

public class TraitManager : Singleton<TraitManager>
{
    [Header("traits")]
    [SerializeField] public List<TraitData> allTraits;

    public TraitData GetTraitById(int id)
    {
        return allTraits.Find(trait => trait.traitId == id);
    }
}
