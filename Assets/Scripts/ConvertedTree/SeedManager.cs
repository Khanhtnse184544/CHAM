using TMPro;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public static SeedManager Instance { get; private set; }

    [Header("UI Reference")]
    public TMP_Text seedQuantityText; // Text hiển thị số hạt

    private int seedCount = 0; // Tổng số hạt hiện tại

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // nếu bạn muốn SeedManager tồn tại giữa các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateSeedUI();
    }

    /// <summary>
    /// Gọi để thêm số lượng hạt giống.
    /// </summary>
    public void AddSeeds(int amount)
    {
        seedCount += amount;
        UpdateSeedUI();
    }

    /// <summary>
    /// Gọi để trừ hạt giống nếu cần.
    /// </summary>
    public void SubtractSeeds(int amount)
    {
        seedCount -= amount;
        seedCount = Mathf.Max(seedCount, 0);
        UpdateSeedUI();
    }

    /// <summary>
    /// Lấy tổng số hạt hiện tại.
    /// </summary>
    public int GetSeedCount()
    {
        return seedCount;
    }

    private void UpdateSeedUI()
    {
        if (seedQuantityText != null)
        {
            seedQuantityText.text = "x" + seedCount;
        }
    }
}
