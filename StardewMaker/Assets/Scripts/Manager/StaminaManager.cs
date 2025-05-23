using UnityEngine;

public class StaminaManager : Singleton<StaminaManager>
{
    [Header("Stamina Settings")]
    public int maxStamina = 10; // 전체 스태미나 칸 수

    public StaminaState[] staminaStates;

    protected override void Awake()
    {
        base.Awake();
        if (!isValid) return;

        InitializeStamina();
    }

    public void InitializeStamina()
    {
        staminaStates = new StaminaState[maxStamina]; // 각 반칸의 상태 저장

        for (int i = 0; i < staminaStates.Length; i++)
        {
            staminaStates[i] = StaminaState.Full;
        }

        StaminaUI.Instance.InitializeUI(maxStamina);
    }

    private void Start()
    {
        SaveManager.Instance.LoadStamina();
    }

    // 스태미나 소모 (반칸씩)
    public bool ConsumeStamina()
    {
        // 마지막(오른쪽)부터 Full/Half 중 첫 칸 찾기
        for (int i = staminaStates.Length - 1; i >= 0; i--)
        {
            if (staminaStates[i] == StaminaState.Full)
            {
                staminaStates[i] = StaminaState.Half;
                UpdateStaminaUI();
                return true;
            }
            else if (staminaStates[i] == StaminaState.Half)
            {
                staminaStates[i] = StaminaState.Empty;
                UpdateStaminaUI();
                return true;
            }
        }

        StaminaUI.Instance.ShakeUI();
        return false;
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
                    didRecover = true;
                    break;
                }
                else if (staminaStates[i] == StaminaState.Half)
                {
                    staminaStates[i] = StaminaState.Full;
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

        UpdateStaminaUI();
    }

    public void UpdateStaminaUI()
    {
        StaminaUI.Instance.UpdateStaminaUI(staminaStates);
    }

    public bool CanConsumeStamina()
    {
        for (int i = staminaStates.Length - 1; i >= 0; i--)
        {
            if (staminaStates[i] == StaminaState.Full || staminaStates[i] == StaminaState.Half)
            {
                return true;
            }
        }

        StaminaUI.Instance.ShakeUI();
        return false;
    }


    //// 테스트용 입력
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        ConsumeStamina();
    //        //StaminaUI.Instance.ShakeUI();
    //    }

    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        RecoverStamina(1);
    //    }
    //}
}