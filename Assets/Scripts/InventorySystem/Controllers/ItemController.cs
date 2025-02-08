using UnityEngine;

/// <summary>
/// Controls item behavior, allowing it to be picked up by the player.
/// </summary>
public class ItemController : MonoBehaviour, IPickable
{
    /// <summary>
    /// The data representing this item.
    /// </summary>
    [SerializeField] private _ItemData data;

    private void Start()
    {
        gameObject.name = data.itemName;
        GetComponent<MeshRenderer>().material = data.material;
    }

    /// <summary>
    /// Attempts to pick up the item and add it to the inventory.
    /// </summary>
    /// <param name="inventory">The player's inventory controller.</param>
    public void PickUp(InventoryController inventory)
    {
        if (inventory.HasSpaceAvailable())
        {
            inventory.AddItem(data);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Detects when the player enters the item's trigger area and attempts to pick it up.
    /// </summary>
    /// <param name="collision">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out InventoryController inventory))
            {
                PickUp(inventory);
            }
        }
    }
}
