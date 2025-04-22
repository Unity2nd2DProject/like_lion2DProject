using System;
using System.Linq;
using UnityEngine;

public class CurrentToolManager : MonoBehaviour
{
    public static CurrentToolManager Instance;
    public ItemData[] tools;
    public ItemData currentTool;
    public Sprite defaultIcon;
    private int index = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }    
    public void NextTool()
    {
        index++;
        if (index >= tools.Length)
        {
            index = -1;
        }

        currentTool = (index == -1) ? null : tools[index];

        if (index == -1)
        {
            CurrentToolUI.Instance.SetIcon(defaultIcon);
        }
        else
        {
            CurrentToolUI.Instance.SetIcon(currentTool.icon);
        }
    }
}
