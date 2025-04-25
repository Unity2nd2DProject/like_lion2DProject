using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingSceneManager : MonoBehaviour
{
    public GameObject toggleButton;
    public GameObject CookingUI;


    private void Awake()
    {
        CookingUI.SetActive(false);
        Initialize();
    }

    private void Initialize()
    {
        toggleButton.GetComponent<Button>().onClick.AddListener(ToggleCookingUI);
    }

    private void ToggleCookingUI()
    {
        CookingUI.SetActive(!CookingUI.activeSelf);
    }
}
