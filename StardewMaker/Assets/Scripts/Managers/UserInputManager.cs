
using UnityEngine;

public class UserInputManager : Singleton<UserInputManager>
{
    private string TAG = "[UserInputManager]";
    public InputActions inputActions;

    protected override void Awake()
    {
        inputActions = new InputActions();

        base.Awake();
    }

    void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.Player.Enable();
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        inputActions.UI.Disable();
        inputActions.Player.Disable();
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.UI:
                inputActions.UI.Enable();
                inputActions.Player.Disable();
                break;
            case GameState.PLAYING:
                inputActions.UI.Disable();
                inputActions.Player.Enable();
                break;
        }
    }
}
