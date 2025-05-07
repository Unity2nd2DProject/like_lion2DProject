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
        List<SavedTree> savedTrees = new List<SavedTree>();
        foreach (var tree in trees)
        {
            savedTrees.Add(new SavedTree
            {
                position = tree.transform.position,
                currentHits = tree.GetCurrentHits(),
                isActive = tree.gameObject.activeSelf
            });
        }
        return savedTrees;
    }

    public List<SavedTree> NextDayTrees()
    {
        List<SavedTree> savedTrees = new List<SavedTree>();
        foreach (var tree in trees)
        {
            savedTrees.Add(new SavedTree
            {
                position = tree.transform.position,
                currentHits = 0,
                isActive = true
            });
        }
        return savedTrees;
    }

    public void LoadTrees(List<SavedTree> savedTrees)
    {
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
