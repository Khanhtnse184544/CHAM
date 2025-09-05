using UnityEngine;
using UnityEngine.UI;

public class HarvestButtonUI : MonoBehaviour
{
    private InteractableObject targetTree;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClickHarvest);
        }
        else
        {
            Debug.LogError("❌ HarvestButtonUI không tìm thấy Button component!");
        }
    }

    public void Init(InteractableObject tree)
    {
        targetTree = tree;
    }

    private void OnClickHarvest()
    {
        if (targetTree != null)
        {
            targetTree.Harvest();
            Destroy(gameObject); // Ẩn nút sau khi thu hoạch
        }
        else
        {
            Debug.LogWarning("⚠ HarvestButtonUI không có targetTree!");
        }
    }
}
