using UnityEngine;
using UnityEngine.EventSystems;

public class XPBarManager : MonoBehaviour
{
    public static XPBarManager Instance;

    public GameObject xpBarPrefab;
    private XPBarUI xpBarUI;
    private GameObject xpBarInstance;

    private TreeXPData currentTree;

    void Awake()
    {
        Instance = this;
    }

    public void ShowXPBar(TreeXPData tree)
    {
        if (xpBarInstance == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            xpBarInstance = Instantiate(xpBarPrefab, canvas.transform);
            xpBarUI = xpBarInstance.GetComponent<XPBarUI>();
        }

        currentTree = tree;
        xpBarInstance.SetActive(true);
        UpdateXP(tree.level, tree.currentXP, tree.maxXP, tree.treeName);
    }

    public void HideXPBar()
    {
        if (xpBarInstance != null)
            xpBarInstance.SetActive(false);
        currentTree = null;
    }

    public void UpdateXP(int level, int currentXP, int maxXP, string treeName)
    {
        if (xpBarUI != null)
            xpBarUI.UpdateXP(level, currentXP, maxXP, treeName);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
            {
                HideXPBar();
            }
            // Chỉ ẩn nếu click ra ngoài (nền) chứ KHÔNG ẩn khi click UI
            if (!IsPointerOverUI())
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider == null || hit.collider.GetComponent<TreeXPData>() == null)
                    HideXPBar();
            }
        }

        if (xpBarInstance != null && xpBarInstance.activeSelf && currentTree != null)
        {
            SpriteRenderer sr = currentTree.GetComponent<SpriteRenderer>();
            float treeHeight = sr != null ? sr.bounds.size.y : 1.5f;
            Vector3 offset = new Vector3(0, treeHeight + 0.2f, 0);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTree.transform.position + offset);
            xpBarInstance.transform.position = screenPos;
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsTrackingTree(TreeXPData tree)
    {
        return currentTree == tree;
    }

    public bool IsXPBarVisible()
    {
        return xpBarInstance != null && xpBarInstance.activeSelf;
    }

    public void AnimateXPChange(TreeXPData tree, int fromXP, int toXP, bool autoHide = true)
    {
        if (xpBarInstance == null || xpBarUI == null)
            return;

        if (currentTree != tree)
            return;

        xpBarUI.AnimateXP(tree.level, fromXP, toXP, tree.maxXP, tree.treeName, autoHide);
    }
}
