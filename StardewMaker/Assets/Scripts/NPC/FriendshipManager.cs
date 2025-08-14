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

    public FriendshipData GetFriendship(string npcName)
    {
        return friendships.Find(f => f.npcName == npcName);
    }

    public FriendshipData GetOrCreateFriendship(string npcName)
    {
        var data = GetFriendship(npcName);
        if (data == null)
        {
            data = new FriendshipData(npcName);
            friendships.Add(data);
        }
        return data;
    }

    public void AddPoints(string npcName, int amount)
    {
        var data = GetOrCreateFriendship(npcName);
        data.points = Mathf.Clamp(data.points + amount, 0, data.maxPoints);
        Debug.Log($"========== [Friendship] {npcName} 호감도 {amount} 상승! ({data.points}/{data.maxPoints}) ==========");
    }

    public int GetHeartLevel(string npcName)
    {
        var data = GetFriendship(npcName);
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
