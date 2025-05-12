using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    public Button inventoryButton;
    public Button equipmentButton;
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject inventoryDescriptionPanel;
    public GameObject equipmentDescriptionPanel;

    private void Start()
    {
        inventoryButton.onClick.AddListener(ShowInventory);
        equipmentButton.onClick.AddListener(ShowEquipment);

        ShowInventory();
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        inventoryDescriptionPanel.SetActive(true);
        equipmentPanel.SetActive(false);
        equipmentDescriptionPanel.SetActive(false);
    }

    public void ShowEquipment()
    {
        inventoryPanel.SetActive(false);
        inventoryDescriptionPanel.SetActive(false);
        equipmentPanel.SetActive(true);
        equipmentDescriptionPanel.SetActive(true);
    }
}
