using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlantLogManager : MonoBehaviour
{
    public static PlantLogManager Instance;

    [Header("UI Panels")]
    public Transform plantingTreeContent;
    public Transform collectionContent;

    [Header("Prefabs")]
    public GameObject logSlotPrefab;

    private Dictionary<InteractableObject, Image> plantingTrees = new Dictionary<InteractableObject, Image>();
    private Dictionary<Sprite, TMP_Text> collectionStacks = new Dictionary<Sprite, TMP_Text>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Ghi log cây khi vừa trồng
    public void RegisterTree(InteractableObject tree, Sprite treeSprite)
    {
        if (tree == null || treeSprite == null) return;

        if (plantingTrees.ContainsKey(tree))
        {
            UpdateTreeSprite(tree, treeSprite);
            return;
        }

        Image emptySlot = FindEmptySlot(plantingTreeContent);
        if (emptySlot == null)
        {
            GameObject newSlot = Instantiate(logSlotPrefab, plantingTreeContent);
            Image[] slotImages = newSlot.GetComponentsInChildren<Image>();
            emptySlot = (slotImages.Length > 1) ? slotImages[1] : newSlot.GetComponent<Image>();
        }

        emptySlot.sprite = treeSprite;
        emptySlot.preserveAspect = true;
        plantingTrees[tree] = emptySlot;
    }

    // Cập nhật sprite cho cây trong đúng slot
    public void UpdateTreeSprite(InteractableObject tree, Sprite newSprite)
    {
        if (tree != null && plantingTrees.ContainsKey(tree))
        {
            plantingTrees[tree].sprite = newSprite;
        }
    }

    // Lấy slot của cây (dùng khi ReplaceWith)
    public Image GetSlotOfTree(InteractableObject tree)
    {
        if (tree != null && plantingTrees.ContainsKey(tree))
        {
            return plantingTrees[tree];
        }
        return null;
    }

    // Chuyển slot từ cây cũ sang cây mới
    public void ReplaceTree(InteractableObject newTree, Sprite newSprite, Image existingSlot)
    {
        if (newTree == null || newSprite == null || existingSlot == null) return;

        // Xóa key cũ
        List<InteractableObject> keysToRemove = new List<InteractableObject>();
        foreach (var kvp in plantingTrees)
        {
            if (kvp.Value == existingSlot)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        foreach (var key in keysToRemove)
        {
            plantingTrees.Remove(key);
        }

        // Gán cây mới vào slot
        existingSlot.sprite = newSprite;
        existingSlot.preserveAspect = true;
        plantingTrees[newTree] = existingSlot;
    }

    // Xóa cây khỏi log khi thu hoạch
    public void RemoveTree(InteractableObject tree)
    {
        if (tree != null && plantingTrees.ContainsKey(tree))
        {
            Image slotImg = plantingTrees[tree];
            if (slotImg != null) slotImg.sprite = null;
            plantingTrees.Remove(tree);
        }
    }

    // Ghi log vào Collection (tìm slot trống hoặc cộng dồn)
    public void AddCollectionLog(Sprite treeSprite)
    {
        if (treeSprite == null) return;

        // 1. Tìm slot có sprite giống
        foreach (Transform child in collectionContent)
        {
            Image[] images = child.GetComponentsInChildren<Image>();
            if (images.Length > 1)
            {
                Image treeImg = images[1];
                TMP_Text countText = child.GetComponentInChildren<TMP_Text>();

                if (treeImg.sprite == treeSprite)
                {
                    int currentCount = int.TryParse(countText.text, out int val) ? val : 0;
                    countText.text = (currentCount + 1).ToString();
                    return;
                }
            }
        }

        // 2. Tìm slot trống
        foreach (Transform child in collectionContent)
        {
            Image[] images = child.GetComponentsInChildren<Image>();
            if (images.Length > 1)
            {
                Image treeImg = images[1];
                TMP_Text countText = child.GetComponentInChildren<TMP_Text>();

                if (treeImg.sprite == null || treeImg.sprite.name == "placeholder")
                {
                    treeImg.sprite = treeSprite;
                    treeImg.preserveAspect = true;
                    countText.text = "1";
                    return;
                }
            }
        }

        // 3. Nếu không có slot trống, tạo slot mới
        GameObject slot = Instantiate(logSlotPrefab, collectionContent);
        Image[] slotImages = slot.GetComponentsInChildren<Image>();
        Image newTreeImg = (slotImages.Length > 1) ? slotImages[1] : slot.GetComponent<Image>();
        TMP_Text newCountText = slot.GetComponentInChildren<TMP_Text>();

        newTreeImg.sprite = treeSprite;
        newTreeImg.preserveAspect = true;
        newCountText.text = "1";
    }

    // Tìm slot trống trong plantingTreeContent
    private Image FindEmptySlot(Transform content)
    {
        foreach (Transform child in content)
        {
            Image[] images = child.GetComponentsInChildren<Image>();
            if (images.Length > 1)
            {
                Image treeImg = images[1];
                if (treeImg.sprite == null || treeImg.sprite.name == "placeholder")
                {
                    return treeImg;
                }
            }
        }
        return null;
    }
}
