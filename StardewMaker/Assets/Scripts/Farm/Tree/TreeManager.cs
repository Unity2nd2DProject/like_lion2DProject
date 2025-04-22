using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;
    public Tree[] trees;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void NextDay()
    {
        foreach(var tree in trees)
        {
            tree.NexDay();
        }
    }
}
