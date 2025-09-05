using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] tabButtons;
    public int defaultTabIndex = 0;

    public Color normalColor = Color.white;
    public Color selectedColor = new Color32(0x7B, 0x61, 0x43, 0xFF); // #7B6143

    private int currentTabIndex = -1;

    void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => ShowPanel(index));
        }

        ShowPanel(defaultTabIndex);
    }

    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        for (int i = 0; i < tabButtons.Length; i++)
        {
            var colors = tabButtons[i].colors;
            colors.normalColor = (i == index) ? selectedColor : normalColor;
            tabButtons[i].colors = colors;
        }

        currentTabIndex = index;
    }

    public void ShowItemPanel()
    {
        ShowPanel(0); // luôn là panel 0
    }
}
