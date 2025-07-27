using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPlacementManager : MonoBehaviour
{
    public static ItemPlacementManager Instance;

    private GameObject previewObj;
    private GameObject currentPrefab;
    private string allowedTilemapTag;
    private GameObject homeCanvas;
    private bool isPlacing = false;
    private bool canPlace = false;
    private Camera cam;
    private Tilemap targetTilemap;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void StartPlacing(string prefabKey, GameObject canvas, string tilemapTag)
    {
        if (isPlacing) return;

        currentPrefab = PrefabRegistry.Instance.GetPrefab(prefabKey);
        if (currentPrefab == null)
        {
            Debug.LogError("Không tìm thấy prefab với key: " + prefabKey);
            return;
        }

        previewObj = Instantiate(currentPrefab);
        previewObj.GetComponent<Collider2D>().enabled = false;
        SetTransparency(previewObj, 0.5f);

        homeCanvas = canvas;
        allowedTilemapTag = tilemapTag;
        homeCanvas.SetActive(false);
        isPlacing = true;
        canPlace = false;

        StartCoroutine(DelayEnablePlacing());
    }

    IEnumerator DelayEnablePlacing()
    {
        yield return null;
        canPlace = true;
    }

    void Update()
    {
        if (!isPlacing || previewObj == null) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        previewObj.transform.position = mousePos;

        if (canPlace && Input.GetMouseButtonDown(0))
        {
            TryPlace(mousePos);
        }
    }

    void TryPlace(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag(allowedTilemapTag))
        {
            // Lấy Tilemap từ collider (chỉ lấy lần đầu)
            if (targetTilemap == null)
            {
                targetTilemap = hit.collider.GetComponent<Tilemap>();
            }

            if (targetTilemap != null)
            {
                // Chuyển world position → cell → center
                Vector3Int cellPos = targetTilemap.WorldToCell(pos);
                Vector3 cellCenterPos = targetTilemap.GetCellCenterWorld(cellPos);

                // Kiểm tra xem tại ô tile đó đã có object chưa
                Collider2D[] colliders = Physics2D.OverlapPointAll(cellCenterPos);
                foreach (var col in colliders)
                {
                    if (col != null && col.gameObject != previewObj && !col.CompareTag(allowedTilemapTag))
                    {
                        Debug.Log("❌ Đã có vật thể tại ô này, không thể đặt.");
                        CleanupAfterPlace();
                        return;
                    }
                }

                // ✅ Instantiate cây mới
                GameObject newTree = Instantiate(currentPrefab, cellCenterPos, Quaternion.identity);
                var io = newTree.GetComponent<InteractableObject>();
                if (io != null && io.GetSprite() != null)
                {
                    PlantLogManager.Instance.RegisterTree(io, io.GetSprite());
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy Tilemap trong collider!");
                Instantiate(currentPrefab, pos, Quaternion.identity); // fallback
            }
        }
        else
        {
            Debug.Log("Không đúng tilemap, không đặt.");
        }

        CleanupAfterPlace();
    }

    void CleanupAfterPlace()
    {
        Destroy(previewObj);
        isPlacing = false;
        canPlace = false;
        homeCanvas.SetActive(true);
    }

    void SetTransparency(GameObject go, float alpha)
    {
        foreach (var sr in go.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;

            // Đảm bảo hiện lên trên tilemap
            sr.sortingOrder = 5;
        }
    }
}
