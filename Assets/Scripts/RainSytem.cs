using UnityEngine;

public class RainSystem : MonoBehaviour
{
    private ParticleSystem rainParticle;
    private Transform camTransform;

    [Header("Kích thước vùng mưa")]
    public float width = 20f;
    public float height = 1f;
    public float yOffset = 5f;

    [Header("Màu của hạt mưa (ban đầu)")]
    public Color startRainColor = new Color(0.5f, 0.7f, 1f, 0.6f);

    private Material rainMat;
    private bool isRaining = true;

    void Start()
    {
        camTransform = Camera.main.transform;

        GameObject rainGO = new GameObject("RainParticle");
        rainGO.transform.SetParent(null);

        rainParticle = rainGO.AddComponent<ParticleSystem>();
        var main = rainParticle.main;
        main.loop = true;
        main.duration = 5f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 2000;

        main.startLifetime = 1.2f;
        main.startSpeed = new ParticleSystem.MinMaxCurve(12f, 20f);
        main.gravityModifier = 1.0f;
        main.startSize3D = true;
        main.startSizeX = 0.03f;
        main.startSizeY = new ParticleSystem.MinMaxCurve(0.15f, 0.2f); // Hạt ngắn lại
        main.startSizeZ = 0.03f;
        main.startRotation = 0f;

        var emission = rainParticle.emission;
        emission.rateOverTime = 400f; // Mưa dày hơn

        var shape = rainParticle.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(width, height, 1f);

        var renderer = rainParticle.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        renderer.lengthScale = 0.3f;
        renderer.velocityScale = 0.2f;

        // Tạo material để thay đổi màu theo thời gian
        rainMat = new Material(Shader.Find("Particles/Standard Unlit"));
        rainMat.color = startRainColor;
        renderer.material = rainMat;
        renderer.sortingOrder = 100;

        rainParticle.Play();
    }

    void LateUpdate()
    {
        if (rainParticle != null && camTransform != null)
        {
            Vector3 pos = camTransform.position;
            pos.y += yOffset;
            rainParticle.transform.position = pos;

            Camera cam = Camera.main;
            if (cam != null && cam.orthographic)
            {
                float camHeight = cam.orthographicSize * 2f;
                float camWidth = camHeight * cam.aspect;

                var shape = rainParticle.shape;
                shape.scale = new Vector3(camWidth, height, 1f);
            }

            UpdateRainColorOverTime();
        }
    }

    public void ToggleRain()
    {
        isRaining = !isRaining;
        EnableRain(isRaining);
    }

    public void EnableRain(bool enable)
    {
        if (rainParticle != null)
        {
            var emission = rainParticle.emission;
            emission.enabled = enable;
        }
    }

    /// <summary>
    /// Thay đổi màu mưa theo thời gian (gradient loop)
    /// </summary>
    private void UpdateRainColorOverTime()
    {
        if (rainMat != null)
        {
            float t = Mathf.PingPong(Time.time * 0.2f, 1f); // chậm
            Color c = Color.Lerp(new Color(0.3f, 0.6f, 1f, 0.5f), new Color(1f, 1f, 1f, 0.7f), t);
            rainMat.color = c;
        }
    }

    public void UpdateRainColor(Color newColor)
    {
        startRainColor = newColor;
        if (rainMat != null)
        {
            rainMat.color = newColor;
        }
    }
}
