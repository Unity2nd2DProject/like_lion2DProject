using UnityEngine;
using UnityEngine.UI;

public enum StaminaState
{
    Full,
    Half,
    Empty
}

public class StaminaUI : MonoBehaviour
{
    public static StaminaUI Instance { get; private set; }

    [Header("Stamina Settings")]
    public Image[] staminaIcons;
    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;

    private StaminaState[] staminaStates;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // 초기화
        staminaStates = new StaminaState[staminaIcons.Length];
        for (int i = 0; i < staminaStates.Length; i++)
        {
            staminaStates[i] = StaminaState.Full;
            staminaIcons[i].sprite = fullSprite;
        }
    }

    // 스태미나 소모 (반칸씩)
    public void ConsumeStamina()
    {
        // 마지막(오른쪽)부터 Full/Half 중 첫 칸 찾기
        for (int i = staminaStates.Length - 1; i >= 0; i--)
        {
            if (staminaStates[i] == StaminaState.Full)
            {
                staminaStates[i] = StaminaState.Half;
                staminaIcons[i].sprite = halfSprite;
                return;
            }
            else if (staminaStates[i] == StaminaState.Half)
            {
                staminaStates[i] = StaminaState.Empty;
                staminaIcons[i].sprite = emptySprite;
                
                bool allEmpty = true;
                for (int j = 0; j < staminaStates.Length; j++)
                {
                    if (staminaStates[j] != StaminaState.Empty)
                    {
                        allEmpty = false;
                        break;
                    }
                }
                if (allEmpty)
                {
                    Debug.Log("스태미나 고갈됐으니 집으로 돌아가삼");
                }

                return;
            }
        }
    }

    // 스태미나 amount 칸(반칸 단위) 회복
    public void RecoverStamina(int amount)
    {
        for (int a = 0; a < amount; a++)
        {
            // 맨 왼쪽(0번)부터 Empty/Half 중 첫 칸 찾기
            bool didRecover = false;
            for (int i = 0; i < staminaStates.Length; i++)
            {
                if (staminaStates[i] == StaminaState.Empty)
                {
                    staminaStates[i] = StaminaState.Half;
                    staminaIcons[i].sprite = halfSprite;
                    didRecover = true;
                    break;
                }
                else if (staminaStates[i] == StaminaState.Half)
                {
                    staminaStates[i] = StaminaState.Full;
                    staminaIcons[i].sprite = fullSprite;
                    didRecover = true;
                    break;
                }
            }

            if (!didRecover)
            {
                Debug.Log("스태미나 풀임");
                break;
            }
        }
    }

    // 테스트용: 엔터키로 반칸 회복
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RecoverStamina(1);
        }
    }
}
