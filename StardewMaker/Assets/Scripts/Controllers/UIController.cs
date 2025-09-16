using UnityEngine;

public class UIController : MonoBehaviour
{
    private string TAG = "[UIController]";
    private UserInputManager inputManager;

    

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
