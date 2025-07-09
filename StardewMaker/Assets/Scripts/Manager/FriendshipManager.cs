using System.Collections.Generic;
using UnityEngine;

public class FriendshipManager : Singleton<FriendshipManager>
{
    [Header("Friendship Table")]
    [SerializeField] private List<FriendshipData> friendships = new List<FriendshipData>();


    protected override void Awake()
    {
        base.Awake();
    }

    public FriendshipData GetFriendship(string npcId)
    {
        return friendships.Find(f => f.npcName == npcId);
    }

    public FriendshipData GetOrCreateFriendship(string npcId)
    {
        var data = GetFriendship(npcId);
        if (data == null)
        {
            data = new FriendshipData(npcId);
            friendships.Add(data);
        }
        return data;
    }

    public void AddPoints(string npcId, int amount)
    {
        var data = GetOrCreateFriendship(npcId);
        data.points = Mathf.Clamp(data.points + amount, 0, data.maxPoints);
        Debug.Log($"[FriendshipManager] {npcId} 호감도: {data.points}/{data.maxPoints}");
    }

    public int GetHeartLevel(string npcId)
    {
        var data = GetFriendship(npcId);
        return data != null ? data.GetHeartLevel() : 0;
    }

    public List<FriendshipData> GetAllFriendships()
    {
        return friendships;
    }

    public void ResetAll()
    {
        friendships.Clear();
    }

}
