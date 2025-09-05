using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragItem_Simple : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalLocalPosition;
    private Transform originalParent;
    private RectTransform rectTransform;
    [Header("CanvasGroup cần ẩn (alpha = 0)")]
    public CanvasGroup[] canvasesToHide;

    private Transform tempRoot; // Root ngoài UI (không bị ảnh hưởng bởi CanvasGroup)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        // Tạo một Transform tạm ngoài canvas
        var tempRootGO = GameObject.Find("TempDragRoot");
        if (tempRootGO == null)
        {
            tempRootGO = new GameObject("TempDragRoot");
            Canvas canvas = tempRootGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            tempRootGO.AddComponent<CanvasScaler>();
            tempRootGO.AddComponent<GraphicRaycaster>();
        }
        tempRoot = tempRootGO.transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalLocalPosition = rectTransform.localPosition;
        originalParent = transform.parent;

        // Di chuyển item ra ngoài canvas bị ẩn
        transform.SetParent(tempRoot, true);

        // Ẩn tất cả canvas group
        foreach (var canvasGroup in canvasesToHide)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            globalMousePos.z = 0f;
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent, true);
        rectTransform.localPosition = originalLocalPosition;

        foreach (var canvasGroup in canvasesToHide)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 worldPoint2D = new Vector2(worldPos.x, worldPos.y);

        // ✅ Dùng RaycastAll để lấy nhiều object trùng nhau
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint2D, Vector2.zero);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                GameObject hitObj = hit.collider.gameObject;
                Debug.Log("Hit 2D object: " + hitObj.name);

                if (hitObj.GetComponent<InteractableMarker>() != null)
                {
                    var interactable = hitObj.GetComponent<InteractableObject>();
                    if (interactable != null)
                    {
                        interactable.Interact(this.gameObject);
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("No 2D object hit");
        }
    }


}
