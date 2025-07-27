using UnityEngine;
using UnityEngine.EventSystems;

public class OpenClosePanel : MonoBehaviour
{
    public GameObject panel;

    // Static để nhớ panel đang mở duy nhất
    private static GameObject currentlyOpenPanel = null;

    void Update()
    {
        if (panel != null && panel.activeSelf && Input.GetMouseButtonDown(0))
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (!EventSystem.current.IsPointerOverGameObject())
#endif
            {
                HidePanel();
            }
        }
    }

    public void TogglePanel()
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;

            // Nếu đang tắt, thì mở lên và ẩn panel khác (nếu có)
            if (!isActive)
            {
                // Ẩn panel khác nếu đang mở
                if (currentlyOpenPanel != null && currentlyOpenPanel != panel)
                    currentlyOpenPanel.SetActive(false);

                panel.SetActive(true);
                currentlyOpenPanel = panel;
            }
            else
            {
                // Nếu đang mở thì ẩn chính nó
                panel.SetActive(false);
                currentlyOpenPanel = null;
            }
        }
    }

    public void ShowPanel()
    {
        if (panel != null)
        {
            if (currentlyOpenPanel != null && currentlyOpenPanel != panel)
                currentlyOpenPanel.SetActive(false);

            panel.SetActive(true);
            currentlyOpenPanel = panel;
        }
    }

    public void OpenShop()
    {
        if (panel != null)
        {
            if (currentlyOpenPanel != null && currentlyOpenPanel != panel)
                currentlyOpenPanel.SetActive(false);

            panel.SetActive(true);
            panel.GetComponent<ShopUIManager>()?.ShowPanel(0);
            currentlyOpenPanel = panel;
        }
    }

    public void HidePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);

            if (currentlyOpenPanel == panel)
                currentlyOpenPanel = null;
        }
    }
}
