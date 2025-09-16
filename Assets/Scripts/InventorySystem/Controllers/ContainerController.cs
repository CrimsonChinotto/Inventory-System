using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic container that stores items and exposes add/remove events and simple transfer logic.
/// Use this for player inventory, chests, vendors, gather nodes, etc.
/// </summary>
public class ContainerController : MonoBehaviour
{
    public UIContainerPanel View { get; private set; }

    /// <summary>True if this container accepts deposits (others can put items here).</summary>
    public bool AllowDeposit => policy.AllowDeposit;

    /// <summary>True if this container allows withdrawals (others can take items from here).</summary>
    public bool AllowWithdraw => policy.AllowWithdraw;

    /// <summary>Read-only snapshot of items.</summary>
    public IReadOnlyList<_ItemData> Items => items.AsReadOnly();

    /// <summary>Capacity accessor.</summary>
    public int Capacity => maxItems;

    /// <summary>Policy flags for this container (can deposit / can withdraw).</summary>
    [SerializeField] private ContainerPolicy policy;

    private readonly List<_ItemData> items = new();

    /// <summary>Max number of items this container can hold.</summary>
    [SerializeField] private int maxItems = 20;

    /// <summary>Event fired when an item is added to this container instance.</summary>
    public event Action<_ItemData> OnItemAdded;

    /// <summary>Event fired when an item is removed from this container instance.</summary>
    public event Action<_ItemData> OnItemRemoved;

    /// <summary>Event fired when an item is successfully transferred from this -> target.</summary>
    public event Action<ContainerController, ContainerController, _ItemData> OnItemTransferred;

    private void Awake()
    {
        View = GetComponent<UIContainerPanel>();

        if (items.Capacity < maxItems) items.Capacity = maxItems;
    }

    private void Start()
    {
        OnItemAdded += View.AddItem;
        OnItemRemoved += View.RemoveItem;
    }


    /// <summary>
    /// Try to add item to this container
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>True if the Item can be added</returns>
    public bool TryAdd(_ItemData item)
    {
        if (item == null) return false;
        if (!HasSpace()) return false;

        items.Add(item);
        View.AddItem(item);
        return true;
    }

    /// <summary>
    /// Try to remove item to this container
    /// </summary>
    /// <param name="item">Item to remove</param>
    /// <returns>True if the Item can be removed</returns>
    public bool TryRemove(_ItemData item)
    {
        if (item == null) return false;
        bool removed = items.Remove(item);

        if (removed) OnItemRemoved?.Invoke(item);
        return removed;
    }

    /// <summary>
    /// Try to transfer an item from a container to another one
    /// </summary>
    /// <param name="target">Target container</param>
    /// <param name="item">Item to transfer</param>
    /// <returns>True if item can be transferred</returns>
    public bool TryTransferTo(ContainerController target, _ItemData item)
    {
        if (target == null || item == null) return false;

        // check policies
        if (!this.AllowWithdraw) return false;    
        if (!target.AllowDeposit) return false;   

        // basic transfer with rollback
        if (!TryRemove(item)) return false;       
        if (!target.TryAdd(item))
        {
            // rollback if target couldn't accept
            TryAdd(item);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check if the container has space available
    /// </summary>
    /// <returns>True if there is space</returns>
    public bool HasSpace()
    {
        return items.Count < maxItems;
    }
}

/// <summary>
/// Defines whether a container allows depositing and/or withdrawing.
/// </summary>
[System.Serializable]
public struct ContainerPolicy
{
    public bool AllowDeposit;  // can other containers deposit into this?
    public bool AllowWithdraw; // can players withdraw from this?
}
