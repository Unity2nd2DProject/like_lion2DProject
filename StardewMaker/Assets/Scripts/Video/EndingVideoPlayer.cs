using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// 엔딩 타입 열거형 (직업별 구분)
public enum EndingType
{
    None = 0,        // 기본값
    Adventurer = 1,  // 모험가
    Artist = 2,      // 예술가
    Musician = 3,    // 음악가
    Chef = 4,        // 요리사
    Teacher = 5      // 선생님
    // 추후 확장 가능
}

public class EndingVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    [SerializeField] private EndingDecider endingDecider;

    [Header("씬 이동 관련")]
    public Button skipButton;         // 스킵 버튼
    public string nextSceneName;      // 다음 씬 이름

    private string videoPath1, videoPath2;  // 첫 번째, 두 번째 영상 경로
    private bool isFirstVideoPlayed = false; // 첫 번째 영상 재생 완료 여부
    private bool isSkipping = false; // 스킵 처리 중인지 확인하기 위한 변수

    void Start()
    {
        CheckEnding(); // 엔딩 판별
        SetEndingPaths();            // 엔딩 영상 경로 설정
        videoPlayer.loopPointReached += OnVideoEnd;  // 영상 종료 이벤트 등록
        StartCoroutine(PrepareAndPlayVideo());       // 첫 번째 영상 준비 및 재생
        skipButton.onClick.AddListener(SkipVideo);   // 스킵 버튼 이벤트 등록

        GameObject.FindGameObjectWithTag("Player").SetActive(false); // 플레이어 없애기
    }
    public void CheckEnding()
    {
        EndingType ending = endingDecider.DecideEnding();
        Debug.Log($"결정된 엔딩: {ending}"); // 디버그 로그 추가
    }

    // 영상 준비 및 재생 코루틴
    private IEnumerator PrepareAndPlayVideo()
    {
        videoPlayer.url = videoPath1;
        videoPlayer.Prepare();

        // 영상이 준비될 때까지 대기
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // 페이드 인 효과와 함께 영상 재생
        FadeManager.Instance.FadeIn(() =>
        {
            videoPlayer.Play();
        });
    }

    // 엔딩 영상 경로 설정
    void SetEndingPaths()
    {
        EndingType endingType = (EndingType)PlayerPrefs.GetInt("EndingType", 0);
        string jobPrefix = GetJobPrefix(endingType);

        videoPath1 = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos", $"{jobPrefix}_1.mp4");
        videoPath2 = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos", $"{jobPrefix}_2.mp4");
    }

    // 직업별 접두사 반환
    private string GetJobPrefix(EndingType type)
    {
        return type switch
        {
            EndingType.Adventurer => "Adventurer",
            EndingType.Artist => "Artist",
            EndingType.Musician => "Musician",
            EndingType.Chef => "Chef",
            EndingType.Teacher => "Teacher",
            _ => "Default" // 기본 엔딩
        };
    }

    // 영상 종료 이벤트 처리
    void OnVideoEnd(VideoPlayer vp)
    {
        if (!isFirstVideoPlayed)
        {
            isFirstVideoPlayed = true;
            StartCoroutine(PrepareAndPlaySecondVideo());
        }
        else
        {
            FadeManager.Instance.FadeOut(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
    }

    // 두 번째 영상 준비 및 재생 코루틴
    private IEnumerator PrepareAndPlaySecondVideo()
    {
        // 페이드 아웃
        yield return StartCoroutine(FadeOutCoroutine());

        // 첫 번째 영상 정지
        videoPlayer.Stop();

        // 두 번째 영상 준비
        videoPlayer.url = videoPath2;
        videoPlayer.Prepare();

        // 영상이 준비될 때까지 대기
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // 두 번째 영상 재생 시작
        videoPlayer.Play();

        // 잠시 대기 후 페이드 인 (영상이 시작되고 나서 페이드 인 하도록)
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeInCoroutine());
    }

    // 페이드 아웃을 코루틴으로 처리
    private IEnumerator FadeOutCoroutine()
    {
        bool fadeComplete = false;
        FadeManager.Instance.FadeOut(() => fadeComplete = true);
        while (!fadeComplete) yield return null;
    }

    // 페이드 인을 코루틴으로 처리
    private IEnumerator FadeInCoroutine()
    {
        bool fadeComplete = false;
        FadeManager.Instance.FadeIn(() => fadeComplete = true);
        while (!fadeComplete) yield return null;
    }

    // 스킵 버튼 처리
    void SkipVideo()
    {
        // 이미 스킵 처리 중인지 확인하기 위한 변수 추가
        if (!isSkipping)
        {
            StartCoroutine(SkipVideoCoroutine());
        }
    }

    private IEnumerator SkipVideoCoroutine()
    {
        isSkipping = true;

        // 비디오 정지
        videoPlayer.Stop();

        // 페이드 아웃 효과 적용
        yield return StartCoroutine(FadeOutCoroutine());

        // 페이드 아웃이 완료된 후 씬 전환
        SceneManager.LoadScene(nextSceneName);

        isSkipping = false;
    }


}
