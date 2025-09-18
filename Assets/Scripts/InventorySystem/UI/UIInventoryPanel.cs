using TMPro;
using UnityEngine;

public class UIInventoryPanel : UIContainerPanel
{
    [Header("UI")]
    /// <summary>
    /// The interaction panel UI element.
    /// </summary>
    [SerializeField] private GameObject interactionPanel;

    /// <summary>
    /// UI text element displaying the Player health.
    /// </summary>
    [SerializeField] private TextMeshProUGUI healthPoints;

    protected override void Start()
    {
        base.Start();
        PlayerBase.OnHealthChanged += SetHPInterface;
    }

    private void OnEnable()
    {
        SetHPInterface();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerBase.OnHealthChanged-= SetHPInterface;
    }

    protected override void SetSelectedItem(UIContainerItem item)
    {
        base.SetSelectedItem(item);

        interactionPanel.SetActive(true);
    }

    protected override void ResetSelectedItem()
    {
        base.ResetSelectedItem();
        interactionPanel.SetActive(false);
    }

    /// <summary>
    /// Set the Health Points player Interface
    /// </summary>
    private void SetHPInterface()
    {
        var player = GameManager.Instance.player;

        healthPoints.text = $"HP: {player.CurrentHealth.ToString()} / {player.MaxHealth.ToString()}";
    }
}
