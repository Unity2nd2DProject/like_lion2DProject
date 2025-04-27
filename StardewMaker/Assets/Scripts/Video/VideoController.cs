using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoController : MonoBehaviour
{
    [Header("비디오 재생 관련")]
    public VideoPlayer videoPlayer;   // 비디오 플레이어 컴포넌트
    public Button skipButton;         // 스킵 버튼
    public Image fadeImage;           // 페이드 아웃용 검정색 이미지
    public float fadeDuration = 1.5f; // 페이드 아웃 지속 시간 (초)

    [Header("씬 이동 관련")]
    public string nextSceneName;      // 이동할 다음 씬 이름 (인스펙터에서 입력)

    private void Start()
    {

        // 영상이 끝나면 호출될 이벤트 등록
        videoPlayer.loopPointReached += OnVideoEnd;

        // 스킵 버튼에 클릭 이벤트 등록
        skipButton.onClick.AddListener(SkipVideo);

        // 처음에는 스킵 버튼과 비디오를 비활성화
        skipButton.gameObject.SetActive(false);
        videoPlayer.playOnAwake = false;

        // 페이드 이미지 시작 시 완전 투명하게 설정
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);

        // 비디오 준비 시작
        StartCoroutine(PrepareVideo());
    }

    private IEnumerator PrepareVideo()
    {
        // 비디오 준비 시작
        videoPlayer.Prepare();

        // 비디오가 준비될 때까지 대기
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("비디오 로딩 중...");
            yield return null;
        }

        Debug.Log("비디오 로딩 완료!");

        // 비디오 재생 시작
        videoPlayer.Play();

        // 일정 시간 후에 스킵 버튼 표시
        Invoke(nameof(ShowSkipButton), 2.0f);
    }

    // 2초 후 스킵 버튼 활성화
    private void ShowSkipButton()
    {
        skipButton.gameObject.SetActive(true);
    }

    // 영상 재생이 끝났을 때 호출
    private void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    // 스킵 버튼을 눌렀을 때 호출
    private void SkipVideo()
    {
        videoPlayer.Stop(); // 영상 즉시 정지
        StartCoroutine(FadeOutAndLoadScene());
    }

    // 화면을 검게 페이드 아웃시키고, 다음 씬으로 이동
    private IEnumerator FadeOutAndLoadScene()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 다음 씬 이름이 비어있지 않은 경우 씬 이동
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("다음 씬 이름이 설정되지 않았습니다! (nextSceneName 비어있음)");
        }
    }
}
