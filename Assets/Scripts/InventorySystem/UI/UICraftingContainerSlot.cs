using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICraftingContainerSlot : UIContainerSlot
{
    [SerializeField] private _ItemData requiredItem;

    [Header("Requirement UI")]
    [SerializeField] private Image requiredItemIcon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        SetupRequiredItem();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var draggedItem = GetDraggedItem(eventData);
        if (draggedItem == null) return;

        if (!IsCorrectItem(draggedItem)) return;

        HandleDrop(draggedItem);
    }

    private void SetupRequiredItem()
    {
        requiredItemIcon.sprite = requiredItem.sprite;
    }

    /// <summary>
    /// Check if the Item dropped is the one required
    /// </summary>
    /// <param name="item">The dropped item</param>
    /// <returns>True if it's the one required, false, else</returns>
    private bool IsCorrectItem(UIContainerItem item)
    {
        return requiredItem != null && item.Data.id == requiredItem.id;
    }
}
