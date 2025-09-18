using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the inventory panel UI, including item selection, usage, and destruction.
/// </summary>
public class UIContainerPanel : MonoBehaviour
{
    /// <summary>
    /// The currently selected inventory item.
    /// </summary>
    private UIContainerItem selectedItem;

    /// <summary>
    /// Array of inventory slots.
    /// </summary>
    [SerializeField] private UIContainerSlot[] containerSlots;

    [Space]
    [Header("Prefabs")]
    /// <summary>
    /// Prefab for UI inventory items.
    /// </summary>
    [SerializeField] private GameObject itemPrefab;

    [Space]
    [Header("UI")]
    /// <summary>
    /// UI text element displaying the selected item's name.
    /// </summary>
    [SerializeField] private TextMeshProUGUI selectedItemName;

    /// <summary>
    /// Event triggered when an item is used.
    /// </summary>
    public static Action<_ItemData> OnItemUsed;

    /// <summary>
    /// Event triggered when an item is destroyed.
    /// </summary>
    public static Action<_ItemData> OnItemDestroyed;

    public static Action OnContainerOpened;
    public static Action OnContainerClosed;

    /// <summary>
    /// Subscribes to inventory events and hides the inventory panel at startup.
    /// </summary>
    protected virtual void Start()
    {
        UIContainerItem.OnItemSelected += SetSelectedItem;
        UIContainerItem.OnItemDragged += ResetSelectedItem;

        gameObject.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
        UIContainerItem.OnItemSelected -= SetSelectedItem;
        UIContainerItem.OnItemDragged -= ResetSelectedItem;
    }

    /// <summary>
    /// Opens the inventory UI.
    /// </summary>
    public void UIC_Open()
    {
        gameObject.SetActive(true);
        OnContainerOpened?.Invoke();
    }

    /// <summary>
    /// Closes the inventory UI and resets the selected item.
    /// </summary>
    public void UIC_Close()
    {
        ResetSelectedItem();
        gameObject.SetActive(false);
        OnContainerClosed?.Invoke();
    }

    /// <summary>
    /// Uses the selected item if it is consumable.
    /// </summary>
    public void UIC_UseItem()
    {
        if (selectedItem.Data is IConsumable consumable)
        {
            PlayerBase player = GameManager.Instance.player;
            bool isConsumed;
            consumable.Consume(player, out isConsumed);

            if (isConsumed)
            {
                OnItemUsed?.Invoke(selectedItem.Data);
                RemoveSelectedItem();
            }
        }
    }

    /// <summary>
    /// Destroys the selected item and triggers the destruction event.
    /// </summary>
    public void UIC_DestroyItem()
    {
        OnItemDestroyed?.Invoke(selectedItem.Data);
        RemoveSelectedItem();
    }

    /// <summary>
    /// Adds a new item to the first available inventory slot.
    /// </summary>
    /// <param name="item">The item data to add.</param>
    public void AddItem(_ItemData item)
    {
        UIContainerSlot freeSlot = GetFirstFreeSlot();

        if (freeSlot != null)
        {
            var newUIItem = Instantiate(itemPrefab);
            newUIItem.transform.SetParent(freeSlot.transform, false);
            freeSlot.Fill();
            newUIItem.GetComponent<UIContainerItem>().Setup(item);
        }
    }

    /// <summary>
    /// Removes a specific item from the UI by data.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    public void RemoveItem(_ItemData item)
    {
        foreach (var slot in containerSlots)
        {
            var uiItem = slot.GetComponentInChildren<UIContainerItem>();
            if (uiItem != null && uiItem.Data == item)
            {
                slot.Empty();
                Destroy(uiItem.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Removes the selected item as the currently selected item and updates UI.
    /// </summary>
    public void RemoveSelectedItem()
    {
        UIContainerItem itemToRemove = selectedItem;
        itemToRemove.CurrentSlot.Empty();
        ResetSelectedItem();
        Destroy(itemToRemove.gameObject);
    }

    /// <summary>
    /// Sets the given item as the currently selected item and updates UI.
    /// </summary>
    /// <param name="containerItem">The item to select.</param>
    protected virtual void SetSelectedItem(UIContainerItem containerItem)
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponentInParent<UIContainerSlot>().SetAsInactive();
        }

        selectedItem = containerItem;
        selectedItem.GetComponentInParent<UIContainerSlot>().SetAsActive();
    }

    /// <summary>
    /// Resets the selected item and hides the interaction panel.
    /// </summary>
    protected virtual void ResetSelectedItem()
    {
        if (selectedItem != null)
        {
            var container = selectedItem.GetComponentInParent<UIContainerSlot>();
            if (container != null) { container.SetAsInactive(); }
        }

        selectedItem = null;
    }

    /// <summary>
    /// Finds the first available inventory slot.
    /// </summary>
    /// <returns>The first free UIInventorySlot or null if none are available.</returns>
    private UIContainerSlot GetFirstFreeSlot()
    {
        foreach (UIContainerSlot slot in containerSlots)
        {
            if (!slot.IsFull)
            {
                return slot;
            }
        }

        return null;
    }
}
