using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConvertedTreeSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image treeImage;
    public TMP_Text quantityText;
    public Button claimButton;
    public Image claimButtonImage;

    [Header("Sprites")]
    public Sprite claimableSprite;
    public Sprite notClaimableSprite;

    [Header("Settings")]
    public int requiredQuantity = 20;
    public Transform claimSpawnPoint;
    public Transform seedBagIconTarget; // Tự động gán qua UIManager


    private void Start()
    {

        // Nếu chưa gán thì lấy từ UIManager
        if (seedBagIconTarget == null && UIManager.Instance != null)
            seedBagIconTarget = UIManager.Instance.seedBagIconTransform;

        // Đăng ký sự kiện click claim
        claimButton.onClick.AddListener(OnClaimClicked);

        // Đăng ký event khi collection thay đổi
        PlantLogManager.Instance.OnCollectionChanged += UpdateClaimButtonState;

        // Cập nhật UI ban đầu
        UpdateClaimButtonState();

    }

    //private void Update()
    //{
    //    UpdateClaimButtonState();
    //}

    public void UpdateClaimButtonState()
    {
        int currentAmount = PlantLogManager.Instance.GetCollectedAmount(treeImage.sprite);

        // So sánh số lượng
        bool isEnough = currentAmount >= requiredQuantity;

        claimButton.interactable = isEnough;
        quantityText.text = "x" + requiredQuantity;

        // Đổi sprite nút
        if (claimButtonImage != null)
        {
            claimButtonImage.sprite = isEnough ? claimableSprite : notClaimableSprite;
        }
    }


    //private int GetCollectedAmount(Sprite targetSprite)
    //{
    //    foreach (Transform child in PlantLogManager.Instance.collectionContent)
    //    {
    //        Image[] images = child.GetComponentsInChildren<Image>();
    //        if (images.Length > 0)
    //        {
    //            Image treeImg = images[1];
    //            TMP_Text countText = child.GetComponentInChildren<TMP_Text>();

    //            if (treeImg.sprite != null && targetSprite != null &&
    //                treeImg.sprite.name == targetSprite.name)
    //            {
    //                int.TryParse(countText.text, out int count);
    //                Debug.Log(countText.text);
    //                return count;
    //            }
    //        }
    //    }
    //    return 0;
    //}


    //private void SubtractCollectedAmount(Sprite targetSprite, int subtractAmount)
    //{
    //    foreach (Transform child in PlantLogManager.Instance.collectionContent)
    //    {
    //        Image[] images = child.GetComponentsInChildren<Image>();
    //        if (images.Length > 1)
    //        {
    //            Image treeImg = images[1];
    //            TMP_Text countText = child.GetComponentInChildren<TMP_Text>();

    //            if (treeImg.sprite != null && targetSprite != null &&
    //                treeImg.sprite.name == targetSprite.name)
    //            {
    //                int.TryParse(countText.text, out int currentCount);
    //                currentCount -= subtractAmount;
    //                currentCount = Mathf.Max(0, currentCount);

    //                if (currentCount == 0)
    //                {
    //                    // ✅ Reset sprite và text về trạng thái "slot trống"
    //                    treeImg.sprite = null; // hoặc đặt placeholder sprite nếu cần
    //                    countText.text = "";

    //                    LayoutRebuilder.ForceRebuildLayoutImmediate(
    //                        PlantLogManager.Instance.collectionContent.GetComponent<RectTransform>());
    //                }
    //                else
    //                {
    //                    countText.text = currentCount.ToString();
    //                }

    //                // 🔹 Thêm dòng này để báo update
                    
    //                break;
    //            }
    //        }
    //    }
    //}




    // Gọi khi ấn Claim
    public void OnClaimClicked()
    {
        int currentAmount = PlantLogManager.Instance.GetCollectedAmount(treeImage.sprite);
        if (currentAmount >= requiredQuantity)
        {
            PlantLogManager.Instance.SubtractCollectedAmount(treeImage.sprite, requiredQuantity);



            // Gọi hiệu ứng bay từ nút claim vào túi
            var flyParams = new RewardFlyParams
            {
                icon = treeImage.sprite,
                originWorldPosition = claimSpawnPoint != null ? claimSpawnPoint.position : claimButton.transform.position,
                originIsUI = true,
                destination = seedBagIconTarget,
                onFlyComplete = () =>
                {
                    // Chỉ cộng hạt giống sau khi bay xong
                    SeedManager.Instance.AddSeeds(1);
                }
            };

            RewardFlyEffectManager.Instance.PlayFlyEffect(flyParams);

            // TODO: Gửi quà cho người chơi ở đây (item, exp, coin,...)
            Debug.Log("Claimed " + requiredQuantity + " of " + treeImage.sprite.name);
        }
    }


    private void OnDestroy()
    {
        // 🔹 Hủy đăng ký để tránh memory leak
        if (PlantLogManager.Instance != null)
            PlantLogManager.Instance.OnCollectionChanged -= UpdateClaimButtonState;
    }
}
