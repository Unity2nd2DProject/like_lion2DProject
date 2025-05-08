using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXEntry
{
    public string sfxName;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "SFXLibrary", menuName = "Audio/SFX Library", order = 1)]
public class SFXLibrary : ScriptableObject
{
    [SerializeField] private List<SFXEntry> sfxEntries = new List<SFXEntry>();
    private Dictionary<string, AudioClip> sfxDict;

    public void Init()
    {
        if (sfxDict != null) return;

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var entry in sfxEntries)
        {
            if (!sfxDict.ContainsKey(entry.sfxName))
            {
                sfxDict.Add(entry.sfxName, entry.clip);
            }
        }
    }

    public AudioClip GetClip(string name)
    {
        if (sfxDict == null) Init();
        return sfxDict.TryGetValue(name, out var clip) ? clip : null;
    }
}