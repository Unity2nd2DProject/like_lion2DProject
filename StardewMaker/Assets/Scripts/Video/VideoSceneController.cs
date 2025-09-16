using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoSceneController : MonoBehaviour
{
    [Header("비디오 재생 관련")]
    public VideoPlayer videoPlayer;   // 비디오 플레이어 컴포넌트
    public Button skipButton;         // 스킵 버튼
    public float skipDelay = 2f; // 스킵 버튼 활성화 지연 시간
    public float fadeDuration = 1f; // 페이드 아웃 지속 시간

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
            yield return null;
        }

        // 비디오 재생 시작
        videoPlayer.Play();
        StartCoroutine(EnableSkipButton(skipDelay));
    }

    // 일정 시간 후 스킵 버튼 활성화
    private IEnumerator EnableSkipButton(float skipDelay)
    {
        yield return new WaitForSeconds(skipDelay);
        skipButton.gameObject.SetActive(true);
    }

    // 영상 재생이 끝났을 때 호출
    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    // 스킵 버튼을 눌렀을 때 호출
    private void SkipVideo()
    {
        videoPlayer.Stop(); // 영상 즉시 정지
        StartCoroutine(FadeOutAndLoadScene());
    }

    // 페이드 아웃 효과 후 씬 전환
    private IEnumerator FadeOutAndLoadScene()
    {
        // 페이드 아웃 효과 (예: 화면을 검게 만드는 등)
        // 여기서는 간단히 1초 대기하는 것으로 대체
        yield return new WaitForSeconds(1f);
        // 다음 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}
