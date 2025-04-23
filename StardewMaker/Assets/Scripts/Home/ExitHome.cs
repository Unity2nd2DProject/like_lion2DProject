using UnityEngine;

public class ExitHome : MonoBehaviour
{
    private string TAG = "[ExitHome]";

    [SerializeField] private string sceneNameToLoad;
    [SerializeField] private string arrivalPointName;

    public void OnClickBtn()
    {
        FadeManager.Instance.FadeOut(() =>
        {
            GameManager.Instance.changeScene("TownScene");
        });
    }
}
