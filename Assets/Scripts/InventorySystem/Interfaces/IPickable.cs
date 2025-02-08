using UnityEngine;

/// <summary>
/// Interface for objects that can be picked up and added to an inventory.
/// </summary>
public interface IPickable
{
    /// <summary>
    /// Picks up the item and adds it to the specified inventory.
    /// </summary>
    /// <param name="inventory">The inventory to add the item to.</param>
    void PickUp(InventoryController inventory);
}
