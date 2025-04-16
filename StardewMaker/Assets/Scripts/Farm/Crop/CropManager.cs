using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance;

    public List<Crop> crops = new List<Crop>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void NextDay()
    {
        Debug.Log("================= Next Day... =====================");
        foreach (var crop in crops)
        {
            crop.NextDay();
        }
    }
}
