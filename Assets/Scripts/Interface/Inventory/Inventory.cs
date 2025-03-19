using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> items = new List<Item>();
    public Item startingItem;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void Add(Item item)
    {
        items.Add(item);
        InventoryUI.Instance.UpdateUI();
    }

    private void Start()
    {
        Add(startingItem);
    }

    //public void Remove(Item item)
    //{
    //    items.Remove(item);
    //    InventoryUI.Instance.UpdateUI();
    //}
}