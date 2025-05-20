using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemData item { get; private set; }
    public bool isFull;
    public bool slotInUse;

    [Header("EquipmentSlot")]
    [SerializeField]
    private Image itemImage;
    public GameObject selectedShader;
    
    [Header("EquipmentDescription")]
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionName;
    public TMP_Text itemDescriptionText;
    public TMP_Text rarityText;
    public TMP_Text mainStatDesc;
    public TMP_Text additionalStatDesc1;
    public TMP_Text additionalStatDesc2;
    public TMP_Text additionalStatDesc3;
    public TMP_Text additionalStatDesc4;

    public Sprite emptySprite;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;
    private EquipmentManager equipmentManager;
    private PlayerStats playerStats;

    private void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        equipmentManager = ScriptableObject.CreateInstance<EquipmentManager>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    private void OnLeftClick()
    {
        if (thisItemSelected && slotInUse)
        {
            UnEquip();
        }
        else
        {
            inventoryManager.selectedItem = item;
            inventoryManager.DeselectAllSlots();
            thisItemSelected = true;
            selectedShader.SetActive(true);

            ToShowDescription();
        }
    }

    private void OnRightClick()
    {
        UnEquip();
    }

    public void Equip(ItemData itemToEquip)
    {
        if (slotInUse)
        {
            UnEquip();
        }

        item = itemToEquip;
        itemImage.sprite = item.Icon;
        slotInUse = true;

        ToShowDescription();

        if (itemToEquip.equipmentData != null)
        {
            equipmentManager.ApplyEquipmentEffects(itemToEquip.equipmentData);
            playerStats.UpdateEquipmentStats();
        }
    }

    public void UnEquip()
    {
        if (!slotInUse)
            return;
        if (item != null && item.equipmentData != null)
            equipmentManager.RemoveEquipmentEffects(item.equipmentData);
        
        inventoryManager.AddItem(item, 1);
        item = null;
        EmptySlot();
        slotInUse = false;

        playerStats.UpdateEquipmentStats();
    }

    private void ToShowDescription()
    {
        if (item != null)
        {
            itemDescriptionImage.sprite = item.Icon;
            itemDescriptionName.text = item.ItemName;
            itemDescriptionText.text = item.Description;

            if (itemDescriptionImage.sprite == null)
                itemDescriptionImage.sprite = emptySprite;

            if (item.IsEquippable() && item.equipmentData != null)
            {
                rarityText.text = item.equipmentData.rarity.ToString();
                // Основной стат
                if (item.equipmentData.mainStat != null)
                {
                    mainStatDesc.text = $"{item.equipmentData.mainStat.statType,-20}\t{item.equipmentData.mainStat.value,10}";
                }
                else
                {
                    mainStatDesc.text = "";
                }
                // Дополнительные статы
                if (item.equipmentData.additionalStats != null && item.equipmentData.additionalStats.Count > 0)
                {
                    additionalStatDesc1.text = item.equipmentData.additionalStats.Count > 0
                        ? $"{item.equipmentData.additionalStats[0].statType,-20}\t{item.equipmentData.additionalStats[0].value,10}"
                        : "";
                    additionalStatDesc2.text = item.equipmentData.additionalStats.Count > 1
                        ? $"{item.equipmentData.additionalStats[1].statType,-20}\t{item.equipmentData.additionalStats[1].value,10}"
                        : "";
                    additionalStatDesc3.text = item.equipmentData.additionalStats.Count > 2
                        ? $"{item.equipmentData.additionalStats[2].statType,-20}\t{item.equipmentData.additionalStats[2].value,10}"
                        : "";
                    additionalStatDesc4.text = item.equipmentData.additionalStats.Count > 3
                        ? $"{item.equipmentData.additionalStats[3].statType,-20}\t{item.equipmentData.additionalStats[3].value,10}"
                        : "";
                }
                else
                {
                    additionalStatDesc1.text = "";
                    additionalStatDesc2.text = "";
                    additionalStatDesc3.text = "";
                    additionalStatDesc4.text = "";
                }
            }
            else
            {
                mainStatDesc.text = "";
                additionalStatDesc1.text = "";
                additionalStatDesc2.text = "";
                additionalStatDesc3.text = "";
                additionalStatDesc4.text = "";
            }
        }
        else
        {
            itemDescriptionImage.sprite = emptySprite;
            itemDescriptionName.text = "";
            itemDescriptionText.text = "";
            rarityText.text = "";
            mainStatDesc.text = "";
            additionalStatDesc1.text = "";
            additionalStatDesc2.text = "";
            additionalStatDesc3.text = "";
            additionalStatDesc4.text = "";
        }
    }

    private void EmptySlot()
    {
        item = null;
        itemImage.sprite = emptySprite;
        itemDescriptionName.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
        rarityText.text = "";
        mainStatDesc.text = "";
        additionalStatDesc1.text = "";
        additionalStatDesc2.text = "";
        additionalStatDesc3.text = "";
        additionalStatDesc4.text = "";
        inventoryManager.DeselectAllSlots();
    }

    public bool IsInUse()
    {
        return slotInUse;
    }
}

