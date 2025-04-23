using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellPopupUI : MonoBehaviour
{
    public TMP_InputField quantityInput;
    public TextMeshProUGUI unitPriceText;
    public Button confirmButton;
    public Button cancelButton;
    public Button increaseButton;
    public Button decreaseButton;

    private ItemSlot currentSlot;
    private ShopManager shopManager;
    private ShopUIController shopUIController;

    private int unitPrice;
    private int maxQty;
    private int currentQty;

    public void Init(ShopManager manager, ShopUIController controller)
    {
        shopManager = manager;
        shopUIController = controller;
    }

    public void Show(ItemSlot slot)
    {
        currentSlot = slot;
        unitPrice = shopManager.GetSellPrice(slot.itemData);
        maxQty = slot.quantity;
        currentQty = 1;

        unitPriceText.text = $"개당 {unitPrice} G";

        UpdateUI();

        quantityInput.onValueChanged.RemoveAllListeners();
        quantityInput.onValueChanged.AddListener(OnQuantityInputChanged);

        increaseButton.onClick.RemoveAllListeners();
        increaseButton.onClick.AddListener(OnIncreaseClicked);

        decreaseButton.onClick.RemoveAllListeners();
        decreaseButton.onClick.AddListener(OnDecreaseClicked);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmClicked);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(OnCancelClicked);

        gameObject.SetActive(true);
    }

    void UpdateUI()
    {
        quantityInput.text = currentQty.ToString();
    }

    void OnQuantityInputChanged(string text)
    {
        int val;
        if (int.TryParse(text, out val))
        {
            currentQty = Mathf.Clamp(val, 1, maxQty);
            UpdateUI();
        }
    }

    void OnIncreaseClicked()
    {
        if (currentQty < maxQty)
        {
            currentQty++;
            UpdateUI();
        }
    }

    void OnDecreaseClicked()
    {
        if (currentQty > 1)
        {
            currentQty--;
            UpdateUI();
        }
    }

    void OnConfirmClicked()
    {
        shopManager.Sell(currentSlot.itemData, currentQty);
        shopUIController.UpdateUI();
        Hide();
    }

    void OnCancelClicked()
    {
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}