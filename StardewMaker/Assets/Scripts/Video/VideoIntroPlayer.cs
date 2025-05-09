using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class VideoIntroPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;


    private void Start()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;
        videoPlayer.loopPointReached += OnVideoEnd;


        // 비디오 재생 시작
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        // 페이드 인 시작
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn(() =>
            {

                videoPlayer.Play();
            });

            // 페이드 완료 대기
            while (FadeManager.Instance.IsFading)
                yield return null;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // 비디오 종료 후 페이드 아웃
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeOut(() =>
            {

                Destroy(this.gameObject); // 또는 다른 씬으로 이동 등의 처리
            });
        }
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoEnd;
    }
}