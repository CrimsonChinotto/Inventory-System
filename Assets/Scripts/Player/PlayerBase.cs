using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// Current health of the player.
    /// </summary>
    public int CurrentHealth { get; private set; }

    /// <summary>
    /// Maximum health of the player.
    /// </summary>
    public int MaxHealth { get { return maxHealth; } }

    /// <summary>
    /// Returns true if the player's health is full (equals max health).
    /// </summary>
    public bool IsFullHealth { get { return (CurrentHealth == MaxHealth); } }

    /// <summary>
    /// Returns true if the player is dead (health is 0 or less).
    /// </summary>
    public bool IsDead { get { return (CurrentHealth <= 0); } }

    [SerializeField] private int maxHealth;

    /// <summary>
    /// Event triggered when health changes.
    /// </summary>
    public static Action OnHealthChanged;

    /// <summary>
    /// Called before the first execution of Update.
    /// Initializes player's health to max value.
    /// </summary>
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    /// <summary>
    /// Heals the player by a given amount, clamping health to the maximum.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void Heal(int amount)
    {
        CurrentHealth = ClampHealth(amount);

        OnHealthChanged?.Invoke();
    }

    /// <summary>
    /// Applies damage to the player, clamping health to a minimum of 0.
    /// </summary>
    /// <param name="amount">Amount of damage to apply.</param>
    public void TakeDamage(int amount)
    {
        CurrentHealth = ClampHealth(-amount);

        OnHealthChanged?.Invoke();

        if (IsDead)
        {
            // Handle death logic (e.g., trigger death animation, game over, etc.)
        }
    }

    /// <summary>
    /// Clamps the player's health within the valid range (0 to maxHealth).
    /// </summary>
    /// <param name="amount">The health change to be applied.</param>
    /// <returns>The clamped health value.</returns>
    private int ClampHealth(int amount)
    {
        return Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
    }
}
