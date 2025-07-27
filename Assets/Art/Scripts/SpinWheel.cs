using UnityEngine;
using System.Collections;

public class SpinWheel : MonoBehaviour
{
    public float spinDuration = 4f;
    public AnimationCurve spinEaseCurve; // Dùng để làm mượt
    public float minRotations = 5f;
    public float maxRotations = 8f;

    private bool isSpinning = false;

    public void StartSpin()
    {
        if (!isSpinning)
            StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        isSpinning = true;

        float elapsed = 0f;
        float totalAngle = 360f * Random.Range(minRotations, maxRotations);
        float currentAngle = 0f;

        while (elapsed < spinDuration)
        {
            float t = elapsed / spinDuration;
            float easedT = spinEaseCurve.Evaluate(t); // Làm mượt theo curve
            float angle = Mathf.Lerp(0f, totalAngle, easedT);

            transform.localEulerAngles = new Vector3(0, 0, -angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = new Vector3(0, 0, -totalAngle);
        isSpinning = false;
    }
}
