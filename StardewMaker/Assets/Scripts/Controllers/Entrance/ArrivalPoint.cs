using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrivalPoint : MonoBehaviour
{
    private string TAG = "[ArrivalPoint]";

    [SerializeField] private string arrivalPointName;

    void Start()
    {
        if (arrivalPointName == GameManager.Instance.arrivalPointName) // 플레이어가 소환될 위치 이름이 현재 이 ArrivalPoint의 이름과 같은지
        {
            // Debug.Log($"{TAG}, {arrivalPointName}");
            GameObject go = GameObject.FindGameObjectWithTag("Player"); // 플레이어가 있을 경우에만 동작
            if (go)
            {
                go.transform.position = transform.position; // 플레이어 해당 위치로 이동
                if (SceneManager.GetActiveScene().name.Contains("Town"))
                {
                    GameManager.Instance.SetGameState(TAG, GameState.PLAYING); // Playing으로 input manager 전환

                    // Player 트래킹 붙이기
                    GameObject cameraObject = GameObject.Find("CinemachineCamera");
                    CinemachineCamera virtualCamera = cameraObject.GetComponent<CinemachineCamera>();
                    GameObject playerObject = GameObject.Find("Player");
                    virtualCamera.Follow = playerObject.transform;

                    SoundManager.Instance.PlaySFX("DoorClose");
                }
                else
                {
                    GameManager.Instance.SetGameState(TAG, GameState.UI); // UI로 input manager 전환
                }
            }
            FadeManager.Instance.FadeIn();
        }
    }
}
