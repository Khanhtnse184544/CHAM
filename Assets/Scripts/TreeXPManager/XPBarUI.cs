using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;
    public TMP_Text xpText;
    public TMP_Text levelText;
    public TMP_Text treeNameText;

    private float currentValue = 0f;

    public void UpdateXP(int level, int currentXP, int maxXP, string treeName)
    {
        xpSlider.maxValue = maxXP;

        DOTween.To(() => currentValue, x => {
            currentValue = x;
            xpSlider.value = currentValue;
        }, currentXP, 0.3f);

        if (xpText != null)
            xpText.text = $"{currentXP}/{maxXP}";

        if (levelText != null)
            levelText.text = $"{level}";

        if (treeNameText != null)
            treeNameText.text = treeName;
    }

    public void AnimateXP(int level, int fromXP, int toXP, int maxXP, string treeName, bool autoHide)
    {
        xpSlider.maxValue = maxXP;
        currentValue = fromXP;

        if (treeNameText != null)
            treeNameText.text = treeName;

        if (levelText != null)
            levelText.text = $"{level}";

        // DOTween animation with easing
        DOTween.To(() => currentValue, x =>
        {
            currentValue = x;
            xpSlider.value = currentValue;

            if (xpText != null)
                xpText.text = $"{Mathf.RoundToInt(currentValue)}/{maxXP}";

        }, toXP, 0.8f) // tăng thời gian cho mượt hơn
        .SetEase(Ease.InOutSine) // thêm easing để mượt hơn
        .OnComplete(() =>
        {
            if (autoHide)
            {
                XPBarManager.Instance?.HideXPBar();
            }
        });
    }

}
