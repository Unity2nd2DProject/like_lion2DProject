using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [Tooltip("이 버튼이 클릭될 때 재생할 SFX 이름")]
    public string sfxName;

    private void Awake()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PlaySFX);
    }

    private void PlaySFX()
    {
        SoundManager.Instance.PlaySFX(sfxName);
    }
}
