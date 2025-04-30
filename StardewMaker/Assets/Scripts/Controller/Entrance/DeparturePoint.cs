using System.Collections;
using UnityEngine;

public class DeparturePoint : MonoBehaviour
{
    private string TAG = "[DeparturePoint]";

    [SerializeField] private string sceneNameToLoad; // 로드할 씬 네임
    [SerializeField] private string arrivalPointName; // 소환될 플레이어 위치 이름

    void OnEnable()
    {
        PrincessScene1Controller.OnExitRequested += Departure;
    }

    void OnDisable()
    {
        PrincessScene1Controller.OnExitRequested -= Departure;
    }

    // 지정된 영역으로 들어온 경우
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>()) Departure();
    }

    // Exit 버튼을 클릭하는 경우 등
    public void Departure()
    {
        GameManager.Instance.arrivalPointName = arrivalPointName;
        FadeManager.Instance.FadeOut();
        StartCoroutine(LoadSceneRoutine());
    }

    // fadeDuration 이후 씬 전환
    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(FadeManager.Instance.fadeDuration);
        GameManager.Instance.changeScene(sceneNameToLoad);

        if (sceneNameToLoad == "HomeScene-yh")
        {
            TimeManager.Instance.PauseTime();
        }
        else if (sceneNameToLoad == "TownScene-yh")
        {
            TimeManager.Instance.ResumeTime();
        }
    }
}