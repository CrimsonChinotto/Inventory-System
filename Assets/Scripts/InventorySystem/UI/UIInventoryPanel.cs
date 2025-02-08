using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the inventory panel UI, including item selection, usage, and destruction.
/// </summary>
public class UIInventoryPanel : MonoBehaviour
{
    /// <summary>
    /// The currently selected inventory item.
    /// </summary>
    private UIInventoryItem selectedItem;

    /// <summary>
    /// Array of inventory slots.
    /// </summary>
    [SerializeField] private UIInventorySlot[] inventorySlots;

    [Space]
    [Header("Prefabs")]
    /// <summary>
    /// Prefab for UI inventory items.
    /// </summary>
    [SerializeField] private GameObject itemPrefab;

    [Space]
    [Header("UI")]
    /// <summary>
    /// The interaction panel UI element.
    /// </summary>
    [SerializeField] private GameObject interactionPanel;

    /// <summary>
    /// UI text element displaying the selected item's name.
    /// </summary>
    [SerializeField] private TextMeshProUGUI selectedItemName;

    /// <summary>
    /// UI text element displaying the Player health.
    /// </summary>
    [SerializeField] private TextMeshProUGUI healthPoints;

    /// <summary>
    /// Button used to open the inventory panel.
    /// </summary>
    [SerializeField] private Button openButton;

    /// <summary>
    /// Event triggered when an item is used.
    /// </summary>
    public static Action<_ItemData> OnItemUsed;

    /// <summary>
    /// Event triggered when an item is destroyed.
    /// </summary>
    public static Action<_ItemData> OnItemDestroyed;

    /// <summary>
    /// Subscribes to inventory events and hides the inventory panel at startup.
    /// </summary>
    private void Awake()
    {
        UIInventoryItem.OnItemSelected += SetSelectedItem;
        UIInventoryItem.OnItemDragged += ResetSelectedItem;
        InventoryController.OnItemAdded += AddItem;
        PlayerBase.OnHealthChanged += SetHPInterface;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SetHPInterface();
    }

    private void OnDestroy()
    {
        UIInventoryItem.OnItemSelected -= SetSelectedItem;
        UIInventoryItem.OnItemDragged -= ResetSelectedItem;
        InventoryController.OnItemAdded -= AddItem;
        PlayerBase.OnHealthChanged -= SetHPInterface;
    }

    /// <summary>
    /// Opens the inventory UI.
    /// </summary>
    public void UIC_Open()
    {
        gameObject.SetActive(true);
        openButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Closes the inventory UI and resets the selected item.
    /// </summary>
    public void UIC_Close()
    {
        ResetSelectedItem();
        gameObject.SetActive(false);
        openButton.gameObject.SetActive(true);
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
    private void AddItem(_ItemData item)
    {
        UIInventorySlot freeSlot = GetFirstFreeSlot();

        if (freeSlot != null)
        {
            var newUIItem = Instantiate(itemPrefab);
            newUIItem.transform.SetParent(freeSlot.transform, false);
            freeSlot.Fill();
            newUIItem.GetComponent<UIInventoryItem>().Setup(item);
        }
    }

    /// <summary>
    /// Removes the selected item as the currently selected item and updates UI.
    /// </summary>
    private void RemoveSelectedItem()
    {
        UIInventoryItem itemToRemove = selectedItem;
        itemToRemove.CurrentSlot.Empty();
        ResetSelectedItem();
        Destroy(itemToRemove.gameObject);
    }

    /// <summary>
    /// Sets the given item as the currently selected item and updates UI.
    /// </summary>
    /// <param name="inventoryItem">The item to select.</param>
    private void SetSelectedItem(UIInventoryItem inventoryItem)
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponentInParent<UIInventorySlot>().SetAsInactive();
        }

        selectedItem = inventoryItem;
        selectedItem.GetComponentInParent<UIInventorySlot>().SetAsActive();
        selectedItemName.text = inventoryItem.Data.itemName;
        interactionPanel.SetActive(true);
    }

    /// <summary>
    /// Resets the selected item and hides the interaction panel.
    /// </summary>
    private void ResetSelectedItem()
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponentInParent<UIInventorySlot>().SetAsInactive();
        }

        selectedItem = null;
        interactionPanel.SetActive(false);
    }

    /// <summary>
    /// Finds the first available inventory slot.
    /// </summary>
    /// <returns>The first free UIInventorySlot or null if none are available.</returns>
    private UIInventorySlot GetFirstFreeSlot()
    {
        foreach (UIInventorySlot slot in inventorySlots)
        {
            if (!slot.IsFull)
            {
                return slot;
            }
        }

        return null;
    }

    /// <summary>
    /// Set the Health Points player Interface
    /// </summary>
    private void SetHPInterface()
    {
        var player = GameManager.Instance.player;

        healthPoints.text = $"HP: {player.CurrentHealth.ToString()} / {player.MaxHealth.ToString()}";
    }
}
