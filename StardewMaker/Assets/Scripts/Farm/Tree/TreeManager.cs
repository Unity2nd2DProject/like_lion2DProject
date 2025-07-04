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
            var (type, hits, hasFruit) = tree.GetState();
            list.Add(new SavedTree
            {
                position = tree.transform.position,
                currentHits = hits,
                treeType = type,
                hasFruit = hasFruit
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
                treeType = saved.treeType,
                hasFruit = saved.treeType == TreeType.Fruit // 다음 날 과일 다시 자라게
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
            tree.SetState(data.treeType, data.currentHits, data.hasFruit);
            Debug.Log($"{data.treeType}");
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
