using UnityEngine;

public class TreeXPData : MonoBehaviour
{
    public int level = 0;
    public int currentXP = 0;
    public int maxXP = 100;

    public string treeName = "Default Tree";

    public bool autoGrow = true;
    public int autoXPAmount = 10;
    public float autoXPInterval = 10f;

    private float timer = 0f;
    private bool isFinalStageReached = false;

    private void Update()
    {
        if (autoGrow)
        {
            timer += Time.deltaTime;
            if (timer >= autoXPInterval)
            {
                AddXP(autoXPAmount, false, true); // AutoGrow
                timer = 0f;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isFinalStageReached && XPBarManager.Instance != null)
            XPBarManager.Instance.ShowXPBar(this);
    }

    public void AddXP(int amount, bool isFinalStage = false, bool isFromAutoGrow = false)
    {
        if (isFinalStage)
        {
            XPBarManager.Instance?.HideXPBar();
            isFinalStageReached = true;
        }

        int previousXP = currentXP;
        currentXP += amount;

        bool isBarVisible = XPBarManager.Instance.IsXPBarVisible();
        bool isTracking = XPBarManager.Instance.IsTrackingTree(this);

        // ✅ Nếu KHÔNG phải autoGrow, và cây này chưa được chọn hoặc bị ẩn → hiện XPBar
        if (!isFromAutoGrow && (!isTracking || !isBarVisible))
        {
            XPBarManager.Instance.ShowXPBar(this);
        }

        // Animate, nhưng nếu là autoGrow thì KHÔNG tự ẩn
        XPBarManager.Instance?.AnimateXPChange(this, previousXP, currentXP, !isFromAutoGrow);

        while (currentXP >= maxXP)
        {
            currentXP -= maxXP;
            level++;
            maxXP = 100 + level * 100;

            var io = GetComponent<InteractableObject>();
            if (io != null)
            {
                if (isFinalStage && io.CompareTag("TreeLV3"))
                {
                    io.GrowThenReplace();
                    return;
                }
                else
                {
                    io.OnLevelUp();
                }
            }
        }
    }

}
