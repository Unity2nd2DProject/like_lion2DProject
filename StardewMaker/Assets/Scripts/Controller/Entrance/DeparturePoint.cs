using System.Collections;
using UnityEngine;

public class DeparturePoint : MonoBehaviour
{
    private string TAG = "[DeparturePoint]";

    [SerializeField] private string sceneNameToLoad;
    [SerializeField] private string arrivalPointName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>()) Departure();
    }

    public void Departure()
    {
        GameManager.Instance.arrivalPointName = arrivalPointName;
        FadeManager.Instance.FadeOut();
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(FadeManager.Instance.fadeDuration);
        GameManager.Instance.changeScene(sceneNameToLoad);
    }
}