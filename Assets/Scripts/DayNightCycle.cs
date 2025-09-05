using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightGlobalLight : MonoBehaviour
{
    public Light2D globalLight;
    public Gradient colorGradient;
    public AnimationCurve intensityCurve;

    [Range(0, 24)]
    public float timeOfDay = 0f;
    public float cycleDuration = 60f;

    void Start()
    {
        colorGradient = new Gradient();
        colorGradient.colorKeys = new GradientColorKey[]
{
    new GradientColorKey(new Color32(255, 255, 220, 255), 0f),   // sáng hơn ban đầu
    new GradientColorKey(new Color32(180, 230, 255, 255), 0.25f), // trời trưa
    new GradientColorKey(new Color32(255, 160, 120, 255), 0.5f),  // hoàng hôn
    new GradientColorKey(new Color32(70, 90, 130, 255), 0.75f),   // đêm
    new GradientColorKey(new Color32(255, 255, 240, 255), 1f)     // sáng lại
};

        colorGradient.alphaKeys = new GradientAlphaKey[]
        {
        new GradientAlphaKey(1f, 0f),
        new GradientAlphaKey(1f, 1f)
        };

        intensityCurve = new AnimationCurve(
    new Keyframe(0f, 0.5f),
    new Keyframe(0.25f, 1.5f),
    new Keyframe(0.5f, 0.8f),
    new Keyframe(0.75f, 0.3f),
    new Keyframe(1f, 0.5f)
);

    }

    void Update()
    {
        timeOfDay += Time.deltaTime * (24f / cycleDuration);
        if (timeOfDay > 24f) timeOfDay = 0f;

        float t = timeOfDay / 24f;

        globalLight.color = colorGradient.Evaluate(t);
        globalLight.intensity = intensityCurve.Evaluate(t);
    }
}
