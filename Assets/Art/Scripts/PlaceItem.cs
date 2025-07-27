using UnityEngine;
using UnityEngine.EventSystems;

public class BagItemPlacer : MonoBehaviour, IPointerClickHandler
{
    public string prefabKey;
    public GameObject homeCanvas;
    public string allowedTilemapTag = "FarmTilemap";

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemPlacementManager.Instance.StartPlacing(prefabKey, homeCanvas, allowedTilemapTag);
    }
}
