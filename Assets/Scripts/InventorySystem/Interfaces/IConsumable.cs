using UnityEngine;

/// <summary>
/// Interface for items that can be consumed to trigger an effect.
/// </summary>
public interface IConsumable
{
    /// <summary>
    /// Consumes the item, applying its effect.
    /// </summary>
    /// <param name="player">The player</param>
    /// <param name="isConsumed">Get if the player consumed the item</param>
    void Consume(PlayerBase player, out bool isConsumed);
}
