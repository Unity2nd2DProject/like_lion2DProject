using UnityEngine;

public class TreeManager : Singleton<TreeManager>
{
    //public static TreeManager Instance;

    public Tree[] trees;

    protected override void Awake()
    {
        base.Awake();
    }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //}

    public void NextDay()
    {
        foreach(var tree in trees)
        {
            tree.NexDay();
        }
    }
}
