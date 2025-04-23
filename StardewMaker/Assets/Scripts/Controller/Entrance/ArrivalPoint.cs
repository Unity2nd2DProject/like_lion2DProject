using UnityEngine;

public class ArrivalPoint : MonoBehaviour
{
    private string TAG = "[ArrivalPoint]";

    [SerializeField] private string arrivalPointName;

    void Start()
    {
        if (arrivalPointName == GameManager.Instance.arrivalPointName)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go)
            {
                go.transform.position = transform.position;
                GameManager.Instance.SetGameState(TAG, GameState.PLAYING);
            }
            FadeManager.Instance.FadeIn();
        }
    }
}
