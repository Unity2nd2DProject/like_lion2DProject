using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI uiText;              // 출력할 Text 컴포넌트
    [TextArea(3, 10)]
    public string fullText;          // 전체 문장
    public float typingSpeed = 0.05f; // 글자 하나 나오는 간격(초)

    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        StartTyping();

    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        uiText.text = "";
        foreach (char c in fullText)
        {
            uiText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


}
