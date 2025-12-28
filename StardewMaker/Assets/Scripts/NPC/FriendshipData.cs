using NPC;
using UnityEngine;

[System.Serializable]
public class FriendshipData
{
    public NpcId npcId;
    public int points;
    public int maxPoints = 100;

    public FriendshipData(NpcId npcId)
    {
        this.npcId = npcId;
        points = 20;
    }

    public int GetHeartLevel(int heartPerPoint = 20)
    {
        return points / heartPerPoint;
    }
}
