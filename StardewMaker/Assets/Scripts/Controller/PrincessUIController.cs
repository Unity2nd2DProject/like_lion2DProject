using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrincessUIController : MonoBehaviour
{
    private string TAG = "[PrincessUIController]";
    private UserInputManager inputManager;

    [SerializeField] public TMP_Text dialogText;
    [SerializeField] public GameObject nextBtn;
    [SerializeField] public GameObject princessImage;

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance;
    }

    void Update()
    {
        ESCInput();
    }

    private void ESCInput()
    {
        if (inputManager.inputActions.UI.ESC.WasPressedThisFrame())
        {
            Debug.Log($"{TAG} ESCInput IsPressed");
            //GameManager.Instance.SetGameState(TAG, GameState.PLAYING);
        }
    }


}
