using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class RewardFlyEffectManager : MonoBehaviour
{
    public static RewardFlyEffectManager Instance;

    [Header("Fly Effect")]
    [SerializeField] private GameObject flyIconPrefab;
    [SerializeField] private Canvas uiCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayFlyEffect(RewardFlyParams parameters)
    {
        // Convert world position to screen space if needed
        Vector3 screenPosition = parameters.originIsUI
            ? parameters.originWorldPosition
            : Camera.main.WorldToScreenPoint(parameters.originWorldPosition);

        // Instantiate icon in UI canvas
        GameObject iconObj = Instantiate(flyIconPrefab, uiCanvas.transform);
        iconObj.transform.position = screenPosition;

        // Set sprite
        Image iconImage = iconObj.GetComponent<Image>();
        if (iconImage != null && parameters.icon != null)
        {
            iconImage.sprite = parameters.icon;
            iconImage.SetNativeSize();
        }

        // Optional: scale down a bit at start
        iconObj.transform.localScale = Vector3.one * 0.3f;

        // Animate fly and scale down at the same time
        DG.Tweening.Sequence flySequence = DOTween.Sequence();
        flySequence.Join(iconObj.transform.DOMove(parameters.destination.position, parameters.flyDuration).SetEase(Ease.OutQuad));
        flySequence.Join(iconObj.transform.DOScale(Vector3.one * 0.00001f, parameters.flyDuration).SetEase(Ease.InQuad));

        flySequence.OnComplete(() =>
        {
            Destroy(iconObj);
            PlayPopEffect(parameters.destination);
            parameters.onFlyComplete?.Invoke();
        });
    }



    private void PlayPopEffect(Transform target)
    {
        // Scale punch on target
        target.DOPunchScale(Vector3.one * 0.2f, 0.3f, 8, 1);

        // Also punch text if exists
        TMP_Text text = target.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 8, 1);
        }
    }
}
