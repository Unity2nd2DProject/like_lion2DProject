using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeManager : Singleton<TreeManager>
{
    public GameObject treeDarkPrefab;
    public GameObject treeLightPrefab;
    public List<Tree> trees = new List<Tree>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void NextDay()
    {
        foreach(var tree in trees)
        {
            tree.NexDay();
        }
    }

    public List<SavedTree> SaveTrees()
    {
        List<SavedTree> list = new List<SavedTree>();
        foreach (var tree in trees)
        {
            list.Add(new SavedTree
            {
                position = tree.transform.position,
                currentHits = tree.GetCurrentHits(),
                isActive = tree.gameObject.activeSelf
            });
        }
        return list;
    }

    public List<SavedTree> NextDayTrees(List<SavedTree> savedList)
    {
        List<SavedTree> list = new List<SavedTree>();

        foreach (var saved in savedList)
        {
            list.Add(new SavedTree
            {
                position = saved.position,
                currentHits = 0,
                isActive = true
            });
        }
        return list;
    }

    public void LoadTrees(List<SavedTree> savedTrees)
    {
        trees = new List<Tree>();

        foreach (var data in savedTrees)
        {
            GameObject prefab = Random.value < 0.5f ? treeDarkPrefab : treeLightPrefab;

            GameObject obj = Instantiate(prefab, data.position, Quaternion.identity);
            Tree tree = obj.GetComponent<Tree>();
            tree.SetState(data.currentHits, data.isActive);
            trees.Add(tree);
        }
    }

    public void RegisterTree(Tree tree)
    {
        if (!trees.Contains(tree))
        {
            trees.Add(tree);
        }
    }

}
