using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text quantityText;
    public ItemData item { get; set; }
    public int quantity;
    public Sprite emptySprite;

    public void Set(Sprite icon, int have, int need)
    {
        itemIcon.sprite = icon;
        quantityText.text = $"{have}/{need}";
    }

    public void Clear()
    {
        itemIcon.sprite = emptySprite;
        quantityText.text = "";
    }
}