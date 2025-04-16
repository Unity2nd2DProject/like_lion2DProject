using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    public List<Crop> crops = new List<Crop>();

    public void NextDay()
    {
        foreach (var crop in crops)
        {
            crop.NextDay();
        }
    }
}
