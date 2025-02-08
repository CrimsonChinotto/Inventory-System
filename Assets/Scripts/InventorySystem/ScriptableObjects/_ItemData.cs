using UnityEngine;

/// <summary>
/// Represents the data for an item in the inventory system.
/// </summary>
public abstract class _ItemData : ScriptableObject
{
    /// <summary>
    /// The name of the item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// The unique identifier for the item.
    /// </summary>
    public string id;

    /// <summary>
    /// The sprite representing the item in the UI.
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// Material of the In-Game Object
    /// </summary>
    public Material material;
}
