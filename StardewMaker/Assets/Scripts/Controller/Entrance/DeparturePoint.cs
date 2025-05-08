using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeparturePoint : MonoBehaviour
{
    private string TAG = "[DeparturePoint]";

    [SerializeField] private string sceneNameToLoad; // 로드할 씬 네임
    [SerializeField] private string arrivalPointName; // 소환될 플레이어 위치 이름
    public GameObject dialogUI;

    void OnEnable()
    {
        PrincessScene1Controller.OnExitRequested += Departure;
        DialogController.OnExitRequested += Departure;
    }

    void OnDisable()
    {
        PrincessScene1Controller.OnExitRequested -= Departure;
        DialogController.OnExitRequested -= Departure;
    }

    // 지정된 영역으로 들어온 경우
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeManager.Instance.currentHour < 19)
        {
            // Debug.Log($"{TAG} 19시 전에는 들어올 수 없음");
            dialogUI.SetActive(true);
            SoundManager.Instance.PlaySfxDialog();
        }
        else
        {
            if (collision.gameObject.GetComponent<PlayerController>()) Departure();
            SoundManager.Instance.PlaySFX("HomeEnter");
        }
    }

    // Exit 버튼을 클릭하는 경우 등
    public void Departure()
    {
        if (SceneManager.GetActiveScene().name.Contains("Town"))
        {
            TimeManager.Instance.currentHour = 19;
        }
        SoundManager.Instance.PlaySFX("HomeEnter");
        GameManager.Instance.arrivalPointName = arrivalPointName;
        FadeManager.Instance.FadeOut();
        StartCoroutine(LoadSceneRoutine());
    }

    // fadeDuration 이후 씬 전환
    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(FadeManager.Instance.fadeDuration);
        GameManager.Instance.changeScene(sceneNameToLoad);
    }
}