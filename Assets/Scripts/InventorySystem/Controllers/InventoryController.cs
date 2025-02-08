using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private List<_ItemData> inventory;

    [SerializeField] private int maxItems;

    public static Action<_ItemData> OnItemAdded;

    private void Awake()
    {
        UIInventoryPanel.OnItemUsed += RemoveItem;
        UIInventoryPanel.OnItemDestroyed += RemoveItem;
    }
    void Start()
    {
        inventory = new();
    }

    public void AddItem(_ItemData item)
    {
        inventory.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(_ItemData item)
    {
        inventory.Remove(item);
    }

    public bool HasSpaceAvailable()
    {
        return inventory.Count < maxItems;
    }
}
