using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EmotionType_legacy
{
    IDLE,
    HAPPY,
    ANGRY,
    SAD
}

public class DialogView_legacy : MonoBehaviour
{
    private string TAG = "[DialogView]";

    [Header("Dialog View")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] public TMP_Text dialogText;
    [SerializeField] private GameObject nextDialogBtn;
    [SerializeField] public GameObject optionPanel;
    [SerializeField] public GameObject optionButtonPrefab;
    [SerializeField] private GameObject characterImage;
    [SerializeField] public TMP_Text characterName;

    private NPCDialog_legacy npcDialog;


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
    public void ChangeCharaterImage(EmotionType_legacy emotionType)
    {
        Sprite newSprite = npcDialog.idleImg;
        switch (emotionType)
        {
            case EmotionType_legacy.IDLE:
                newSprite = npcDialog.idleImg;
                break;
            case EmotionType_legacy.HAPPY:
                newSprite = npcDialog.happyImg;
                break;
            case EmotionType_legacy.ANGRY:
                newSprite = npcDialog.angryImg;
                break;
            case EmotionType_legacy.SAD:
                newSprite = npcDialog.sadImg;
                break;
        }
        characterImage.GetComponent<Image>().sprite = newSprite;
    }

    // 옵션 버튼 생성
    public GameObject CreateOptionButton(string text)
    {
        GameObject go = Instantiate(optionButtonPrefab, optionPanel.transform);
        go.GetComponentInChildren<TMP_Text>().text = text;
        return go;
    }

    // 옵션 버튼 삭제
    public void DeleteAllOptionButton()
    {
        foreach (Transform child in optionPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // NPC Dialog 설정(id, sprite)
    public void SetNPCDialog(NPCDialog_legacy npcDialog)
    {
        this.npcDialog = npcDialog;
    }

    // NPC 이름 설정
    public void SetCharacterName(string name)
    {
        characterName.text = name;
    }

    // NPD 얼굴 이미지 보이기
    public void EnableNPCImage(bool sw)
    {
        characterImage.SetActive(sw);
    }
}
