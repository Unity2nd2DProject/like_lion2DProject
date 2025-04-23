using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class TextTool
{
    private static string TAG = "[TextTool]";

    private const float delayWord = 0.08f;
    private const float delayWordFast = 0.008f;
    private const float delaySentence = 0.5f;
    private const float delayLine = 0.8f;
    private const float delayDots = 0.5f;

    public static IEnumerator PrintTmpText(TMP_Text tmp, string text, Func<bool> skipCheck, float delay = delayWord)
    {
        StringBuilder sb = new();
        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            tmp.text = sb.ToString();
            float d = skipCheck() ? delayWordFast : delay;
            yield return new WaitForSeconds(d);
        }
    }
}
