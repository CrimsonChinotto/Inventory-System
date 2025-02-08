using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a slot in the inventory UI. Handles item drops and visual states.
/// </summary>
public class UIInventorySlot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// The background color when the slot is active.
    /// </summary>
    [SerializeField] private Color activeColor;

    /// <summary>
    /// The background color when the slot is inactive.
    /// </summary>
    [SerializeField] private Color inactiveColor;

    /// <summary>
    /// The background image of the slot.
    /// </summary>
    private Image background;

    /// <summary>
    /// Indicates whether the slot is occupied by an item.
    /// </summary>
    public bool IsFull { get; private set; }

    /// <summary>
    /// Initializes the slot by retrieving the Image component.
    /// </summary>
    private void Awake()
    {
        background = GetComponent<Image>();
    }

    /// <summary>
    /// Checks if the slot already contains an item at the start.
    /// </summary>
    private void Start()
    {
        IsFull = GetComponentInChildren<UIInventoryItem>() != null;
    }

    /// <summary>
    /// Handles the drop event when an item is dragged onto this slot.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            UIInventoryItem item = eventData.pointerDrag.GetComponent<UIInventoryItem>();
            item.parentAfterDrag = transform;
            item.CurrentSlot = this; // Set the new parent slot
            IsFull = true;
        }
    }

    /// <summary>
    /// Marks the slot as occupied.
    /// </summary>
    public void Fill()
    {
        IsFull = true;
    }

    /// <summary>
    /// Marks the slot as empty.
    /// </summary>
    public void Empty()
    {
        IsFull = false;
    }

    /// <summary>
    /// Sets the slot's background color to indicate it is active.
    /// </summary>
    public void SetAsActive()
    {
        background.color = activeColor;
    }

    /// <summary>
    /// Sets the slot's background color to indicate it is inactive.
    /// </summary>
    public void SetAsInactive()
    {
        background.color = inactiveColor;
    }
}
