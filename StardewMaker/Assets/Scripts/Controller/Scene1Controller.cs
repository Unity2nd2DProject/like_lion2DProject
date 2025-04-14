using UnityEngine;

public class Scene1Controller : MonoBehaviour
{
    private string TAG = "[Scene1Controller]";

    private void Start()
    {
        Debug.Log($"{TAG} Scene1 시작");
        GameManager.Instance.SetGameState(TAG, GameState.PLAYING);
    }
}
