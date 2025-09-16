using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a slot in the inventory UI. Handles item drops and visual states.
/// </summary>
public class UIContainerSlot : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// Parent container
    /// </summary>
    public UIContainerPanel Container { get; private set; }

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
        Container = transform.parent.gameObject.GetComponentInParent<UIContainerPanel>();
    }

    /// <summary>
    /// Checks if the slot already contains an item at the start.
    /// </summary>
    private void Start()
    {
        IsFull = GetComponentInChildren<UIContainerItem>() != null;
    }

    /// <summary>
    /// Handles the drop event when an item is dragged onto this slot.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        // Get the item being dragged
        UIContainerItem draggedItem = eventData.pointerDrag.GetComponent<UIContainerItem>();
        if (draggedItem == null) return;

        draggedItem.DroppedSuccessfully = false;

        // Get source and target ContainerControllers
        var sourceContainer = draggedItem.LastSlot?.Container.GetComponent<ContainerController>();
        var targetContainer = Container.GetComponent<ContainerController>();

        // No valid source/target
        if (sourceContainer == null || targetContainer == null) return;

        // Avoid dropping into same slot
        if (sourceContainer != targetContainer)
        {
            sourceContainer.TryTransferTo(targetContainer, draggedItem.Data);
        }

        draggedItem.DroppedSuccessfully = true;
        draggedItem.CurrentSlot = this;
        Fill();
        draggedItem.transform.SetParent(transform, false);
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
