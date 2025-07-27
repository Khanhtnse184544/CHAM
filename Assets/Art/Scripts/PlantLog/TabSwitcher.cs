using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    public GameObject plantedTreesPanel;
    public GameObject collectionPanel;

    // Nền tab riêng
    public Image plantedTabBackground;
    public Image collectionTabBackground;

    // Nút tab riêng (text)
    public TMP_Text plantedTabText;
    public TMP_Text collectionTabText;

    // Sprites cho nền
    public Sprite plantedTab_Selected;
    public Sprite plantedTab_Unselected;
    public Sprite collectionTab_Selected;
    public Sprite collectionTab_Unselected;

    // Màu chữ
    public Color selectedTextColor = Color.white;
    public Color unselectedTextColor = new Color32(92, 58, 29, 255); // #5C3A1D

    public void ShowPlantedTreesTab()
    {
        plantedTreesPanel.SetActive(true);
        collectionPanel.SetActive(false);

        // Update background
        plantedTabBackground.sprite = plantedTab_Selected;
        collectionTabBackground.sprite = collectionTab_Unselected;

        // Update text color
        plantedTabText.color = selectedTextColor;
        collectionTabText.color = unselectedTextColor;
    }

    public void ShowCollectionTab()
    {
        plantedTreesPanel.SetActive(false);
        collectionPanel.SetActive(true);

        // Update background
        plantedTabBackground.sprite = plantedTab_Unselected;
        collectionTabBackground.sprite = collectionTab_Selected;

        // Update text color
        plantedTabText.color = unselectedTextColor;
        collectionTabText.color = selectedTextColor;
    }


}
