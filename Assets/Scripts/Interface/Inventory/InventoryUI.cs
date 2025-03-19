using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject inventoryPanel;
    //public GameObject equipmentPanel;
    public Transform itemsParent;
    public InventorySlot[] slots;
    public Image itemImage;
    public TMP_Text itemDescription;
    //public Image draggedIcon;
    //public InventorySlot draggedSlot;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        inventoryPanel.SetActive(false);
        //equipmentPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            //equipmentPanel.SetActive(false);
        }
        //if (draggedIcon.enabled)
        //    draggedIcon.transform.position = Input.mousePosition;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.items.Count)
                slots[i].AddItem(Inventory.Instance.items[i]);
            else
                slots[i].ClearSlot();
        }
    }

    public void ShowItemDetails(Item item)
    {
        itemImage.sprite = item.icon;
        itemDescription.text = item.description;
    }

    //public void OpenEquipmentPanel()
    //{
    //    equipmentPanel.SetActive(true);
    //    inventoryPanel.SetActive(false);
    //}

    //public void StartDrag(InventorySlot slot, Sprite sprite)
    //{
    //    draggedSlot = slot;
    //    draggedIcon.sprite = sprite;
    //    draggedIcon.enabled = true;
    //}

    //public void EndDrag()
    //{
    //    draggedSlot = null;
    //    draggedIcon.enabled = false;
    //}
}