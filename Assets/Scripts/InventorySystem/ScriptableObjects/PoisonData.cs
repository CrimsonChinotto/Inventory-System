using UnityEngine;

/// <summary>
/// Represents a poison item that can be consumed to deal damage.
/// </summary>
[CreateAssetMenu(fileName = "New Poison", menuName = "Scriptable Objects/New Poison")]
public class PoisonData : _ItemData, IConsumable
{
    /// <summary>
    /// The amount of damage inflicted when consumed.
    /// </summary>
    [Space(25)]
    public int amountToDamage;

    /// <summary>
    /// Consumes the poison, triggering its effect.
    /// </summary>
    public void Consume(PlayerBase player, out bool isConsumed)
    {
        isConsumed = true;
        Debug.Log($"{itemName} consumed");

        player.TakeDamage(amountToDamage);
    }
}
