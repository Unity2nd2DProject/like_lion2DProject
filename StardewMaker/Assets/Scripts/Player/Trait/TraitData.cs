using System.Collections.Generic;
using UnityEngine;

public enum TraitType
{
    Farming,
    Fishing,
    Hunting,
    Cooking,
    Woodcutting
}

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait/Create New Trait")]
public class TraitData : ScriptableObject
{
    [Header("Info")]
    public TraitType skillType;
    public int traitId;
    public string traitName;
    [TextArea] public string description;

    [Header("Level")]
    public int maxLevel = 20;
    public float baseExpToLevelUp = 10f;
    public float expMultiplier = 1.5f;

    [Header("Effect")]
    public List<TraitEffectData> effects;
}
