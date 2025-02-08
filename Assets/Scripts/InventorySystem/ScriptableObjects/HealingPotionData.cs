using UnityEngine;

/// <summary>
/// Represents a healing potion item that can be consumed to restore health.
/// </summary>
[CreateAssetMenu(fileName = "New Healing Potion", menuName = "Scriptable Objects/New Healing Potion")]
public class HealingPotionData : _ItemData, IConsumable
{
    /// <summary>
    /// The amount of health restored when consumed.
    /// </summary>
    [Space(25)]
    public int amountToHeal;

    public void Consume(PlayerBase player, out bool isConsumed)
    {
        if (!player.IsFullHealth) 
        {
            isConsumed = true;
            player.Heal(amountToHeal);

            Debug.Log($"{itemName} consumed");

            return;
        }

        isConsumed = false;
    }
}
