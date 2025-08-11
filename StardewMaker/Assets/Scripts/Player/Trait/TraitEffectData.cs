using UnityEngine;

public enum TraitEffectType
{
    StatModifier,     // 수치 계열 효과 (ex. 성장속도 증가)
    EventUnlock,      // 이벤트 발생 조건 해금
    ItemUnlock,       // 아이템 or 제작 해금
    RecipeUnlock,     // 요리 레시피 해금
    InteractionBoost, // 딸과 상호작용 시 효과
    CutsceneUnlock    // 컷씬 등 연출 해금
}

public enum StatEffectTarget
{
    CropGrowthSpeed,
    FoodEffectiveness,
    FishingSuccessWindow,
    RareFishChance,
    CriticalChance,
    PickSpeed,
    MoveSpeed,
    DaughterRecovery,
    DaughterStatGainChance
}

[System.Serializable]
public class TraitEffectData
{
    [Header("Info")]
    public TraitEffectType effectType;
    [TextArea] public string description;      // 효과 설명용 (UI 표기용)

    [Header("Stat Effect")]
    public StatEffectTarget statTarget;
    public float effectValue;       // 수치 효과

    [Header("Event Effect")]
    public int unlockLevel = 20;         // 해금형 효과
    public string targetId;         // 예: unlock할 아이템, 이벤트 id 등
}
