using NPC;
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

    private void Start()
    {
        SaveManager.Instance.LoadFriendship();
    }

    public FriendshipData GetFriendship(NpcId npcId)
    {
        return friendships.Find(f => f.npcId == npcId);
    }

    public FriendshipData GetOrCreateFriendship(NpcId npcId)
    {
        var data = GetFriendship(npcId);
        if (data == null)
        {
            data = new FriendshipData(npcId);
            friendships.Add(data);
        }
        return data;
    }

    public void AddPoints(NpcId npcId, int amount)
    {
        var data = GetOrCreateFriendship(npcId);
        data.points = Mathf.Clamp(data.points + amount, 0, data.maxPoints);
        Debug.Log($"========== [Friendship] {npcId} 호감도 {amount} 상승! ({data.points}/{data.maxPoints}) ==========");
        UIManager.Instance.ShowPopup($"{npcId} 호감도 {amount} 상승! ({data.points}/{data.maxPoints})", new Vector3(Screen.width / 2f, Screen.height / 1.2f));
    }

    public int GetHeartLevel(NpcId npcId)
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
