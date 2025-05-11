//using UnityEngine;
//using UnityEngine.UI;

//public class DropItemMenu : MonoBehaviour
//{
//    public GameObject panel;
//    public InputField amountInput;
//    public Button dropButton;
//    public Button cancelButton;

//    private InventoryManager inventoryManager;
//    private ItemData currentItem;
//    private int currentMaxAmount;

//    private void Start()
//    {
//        panel.SetActive(false);
//        inventoryManager = InventoryManager.Instance;

//        dropButton.onClick.AddListener(OnDropClicked);
//        cancelButton.onClick.AddListener(Close);
//    }

//    public void Open(ItemData item, int maxAmount)
//    {
//        currentItem = item;
//        currentMaxAmount = maxAmount;
//        amountInput.text = "1";
//        panel.SetActive(true);
//    }

//    public void Close()
//    {
//        panel.SetActive(false);
//        currentItem = null;
//    }

//    private void OnDropClicked()
//    {
//        if (!int.TryParse(amountInput.text, out int amount))
//            return;

//        amount = Mathf.Clamp(amount, 1, currentMaxAmount);

//        inventoryManager.DropItem(currentItem, amount);
//        inventoryManager.RemoveItem(currentItem, amount);

//        Close();
//    }
//}
