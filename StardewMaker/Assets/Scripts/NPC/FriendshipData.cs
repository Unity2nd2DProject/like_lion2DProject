using UnityEngine;

[System.Serializable]
public class FriendshipData
{
    public string npcName;
    public int points;
    public int maxPoints = 100;

    public FriendshipData(string name)
    {
        npcName = name;
        points = 0;
    }

    public int GetHeartLevel(int heartPerPoint = 20)
    {
        return points / heartPerPoint;
    }
}
