using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents an inventory item in the UI. Supports dragging and clicking.
/// </summary>
public class UIInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    /// <summary>
    /// Stores the original parent transform before dragging.
    /// </summary>
    [HideInInspector] public Transform parentAfterDrag;

    /// <summary>
    /// The data associated with this inventory item.
    /// </summary>
    public _ItemData Data { get; private set; }

    /// <summary>
    /// The slot where this item is currently placed.
    /// </summary>
    public UIInventorySlot CurrentSlot { get; set; }

    /// <summary>
    /// The image component displaying the item sprite.
    /// </summary>
    private Image image;

    /// <summary>
    /// Event triggered when an item is selected (clicked).
    /// </summary>
    public static Action<UIInventoryItem> OnItemSelected;

    /// <summary>
    /// Event triggered when an item starts being dragged.
    /// </summary>
    public static Action OnItemDragged;

    /// <summary>
    /// Initializes the item by getting its image component.
    /// </summary>
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// Finds the slot the item is currently placed in.
    /// </summary>
    private void Start()
    {
        CurrentSlot = GetComponentInParent<UIInventorySlot>();
    }

    /// <summary>
    /// Called when dragging begins. Disables raycast, updates parent, and notifies the slot.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;

        if (CurrentSlot != null)
        {
            CurrentSlot.Empty(); // Notify the slot that it's empty now
            CurrentSlot = null;
        }

        transform.SetParent(transform.root);
    }

    /// <summary>
    /// Called while dragging the item. Moves it with the mouse cursor.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    /// <summary>
    /// Called when dragging ends. Re-enables raycast and restores parent.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    /// <summary>
    /// Called when the item is clicked. Triggers the selection event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemSelected?.Invoke(this);
    }

    /// <summary>
    /// Sets up the item with the given data and updates the sprite.
    /// </summary>
    /// <param name="data">The item data to assign.</param>
    public void Setup(_ItemData data)
    {
        Data = data;
        image.sprite = Data.sprite;
    }
}
