using UnityEngine;

public class UIButtonsHolder : MonoBehaviour
{
    private void Start()
    {
        UIContainerPanel.OnContainerOpened += HideHolder;
        UIContainerPanel.OnContainerClosed += ShowHolder;
    }

    private void OnDestroy()
    {
        UIContainerPanel.OnContainerOpened -= HideHolder;
        UIContainerPanel.OnContainerClosed -= ShowHolder;
    }

    private void ShowHolder()
    {
        gameObject.SetActive(true);
    }

    private void HideHolder()
    {
        gameObject.SetActive(false);
    }
}
