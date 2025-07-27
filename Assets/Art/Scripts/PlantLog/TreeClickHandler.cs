using UnityEngine;

public class TreeClickHandler : MonoBehaviour
{
    public GameObject plantLogPanel;

    public void OnTreeClicked()
    {
        Debug.Log("Tree clicked, showing PlantLogPanel");
        plantLogPanel.SetActive(true);
        plantLogPanel.GetComponent<TabSwitcher>().ShowPlantedTreesTab(); // mặc định vào tab cây đã trồng
    }
}
