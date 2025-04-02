using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    public Button inventoryButton;
    public Button equipmentButton;
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;

    private void Start()
    {
        inventoryButton.onClick.AddListener(ShowInventory);
        equipmentButton.onClick.AddListener(ShowEquipment);

        ShowInventory();
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        equipmentPanel.SetActive(false);
    }

    public void ShowEquipment()
    {
        inventoryPanel.SetActive(false);
        equipmentPanel.SetActive(true);
    }
}
