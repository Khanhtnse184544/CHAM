using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Prefab thay thế sau khi tương tác")]
    public GameObject replacementPrefab;

    [Header("Tăng trưởng")]
    public float growthDuration = 5f;
    public Vector3 finalScale = new Vector3(1.2f, 1.2f, 1.2f);

    [Header("UI Harvest Button (chỉ cần cho LV3)")]
    public GameObject harvestButtonPrefab;
    private GameObject harvestButtonInstance;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Scale nhỏ lúc bắt đầu, nếu là seed
        if (CompareTag("Seed"))
            transform.localScale = Vector3.one * 0.3f;
    }

    private void Update()
    {
        if (harvestButtonInstance != null) UpdateHarvestButtonPosition();
    }

    public Sprite GetSprite() => sr != null ? sr.sprite : null;

    public void Interact(GameObject item)
    {
        string itemName = item.name;
        string tag = this.gameObject.tag;

        if (tag == "Seed" && itemName.Contains("Watercan"))
            ReplaceWith(replacementPrefab);
        else if (tag == "TreeLV1" && itemName.Contains("Fertilizer"))
            ReplaceWith(replacementPrefab);
        else if (tag == "TreeLV2" && item.name.Contains("Pesticide"))
            ReplaceWith(replacementPrefab);
        else if (tag == "TreeLV3" && itemName.Contains("Pesticide"))
            GrowThenReplace();
        else
            Debug.Log("❌ Không đúng loại tương tác");
    }

    private void GrowThenReplace()
    {
        transform.DOScale(finalScale, growthDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                UpdatePlantLogSprite();
                ShowHarvestButton();
            });
    }

    private void ReplaceWith(GameObject newPrefab)
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Transform originalParent = transform.parent;

        int sortingOrder = sr.sortingOrder;
        string sortingLayerName = sr.sortingLayerName;

        // Lấy slot cũ
        Image oldSlot = PlantLogManager.Instance.GetSlotOfTree(this);

        Destroy(gameObject);
        GameObject newObj = Instantiate(newPrefab, pos, rot);

        if (originalParent != null) newObj.transform.SetParent(originalParent);

        SpriteRenderer newRenderer = newObj.GetComponent<SpriteRenderer>();
        if (newRenderer != null)
        {
            newRenderer.sortingOrder = sortingOrder;
            newRenderer.sortingLayerName = sortingLayerName;
        }

        var io = newObj.GetComponent<InteractableObject>();
        if (io != null && newRenderer != null)
        {
            // Gán scale theo cấp độ cây
            float scaleMultiplier = GetScaleMultiplier(newObj.tag);
            io.PlayGrowthAnimation(scaleMultiplier);

            // Dùng lại slot cũ, không tạo mới
            PlantLogManager.Instance.ReplaceTree(io, newRenderer.sprite, oldSlot);
        }
    }

    private float GetScaleMultiplier(string tag)
    {
        if (tag == "TreeLV1") return 0.3f;
        else if (tag == "TreeLV2") return 0.6f;
        else if (tag == "TreeLV3") return 1.2f;
        else return 1.2f;
    }

    public void PlayGrowthAnimation(float targetScale)
    {
        transform.localScale = Vector3.one * 0.1f;
        transform.DOScale(Vector3.one * targetScale, 1f)
            .SetEase(Ease.OutBack);
    }

    private void ShowHarvestButton()
    {
        if (harvestButtonPrefab == null) return;

        if (harvestButtonInstance == null)
        {
            Canvas mainCanvas = FindObjectOfType<Canvas>();
            if (mainCanvas == null) return;

            harvestButtonInstance = Instantiate(harvestButtonPrefab, mainCanvas.transform);
            UpdateHarvestButtonPosition();

            var btnUI = harvestButtonInstance.GetComponent<HarvestButtonUI>();
            if (btnUI != null) btnUI.Init(this);
        }
    }

    private void UpdateHarvestButtonPosition()
    {
        if (sr == null || harvestButtonInstance == null) return;

        float treeHeight = sr.bounds.size.y;
        Vector3 offset = new Vector3(0, treeHeight + 0.2f, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
        harvestButtonInstance.transform.position = screenPos;
    }

    private void UpdatePlantLogSprite()
    {
        if (sr != null)
            PlantLogManager.Instance.UpdateTreeSprite(this, sr.sprite);
    }

    public void Harvest()
    {
        if (CompareTag("TreeLV3") && sr != null)
        {
            PlantLogManager.Instance.AddCollectionLog(sr.sprite);
            PlantLogManager.Instance.RemoveTree(this);
            Destroy(gameObject);
        }
    }
}
