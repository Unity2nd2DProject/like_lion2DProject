using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{
    private string TAG = "[DialogView]";

    [Header("Dialog View")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] public TMP_Text dialogText;
    [SerializeField] private GameObject nextDialogBtn;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject characterImage;


    // dialog 화면
    public void EnableDialogPanel(bool sw)
    {
        dialogPanel.SetActive(sw);
    }

    // 선택 옵션 창
    public void EnableOptionPanel(bool sw)
    {
        optionPanel.SetActive(sw);
    }

    // 다음 대사 버튼
    public void EnableNextDialogBtn(bool sw)
    {
        nextDialogBtn.SetActive(sw);
    }

    // 대사 캐릭터 이미지 교체
    public void ChangeCharaterImage(Sprite sprite)
    {
        characterImage.GetComponent<Image>().sprite = sprite;
    }
}
