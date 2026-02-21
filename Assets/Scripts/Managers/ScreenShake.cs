using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    [SerializeField] Transform cameraTransform;

    Vector3 originalPos;

    void Awake()
    {
        Instance = this;

        if (cameraTransform == null)
        {
            Debug.LogError("ScreenShake: Camera Transform not assigned!");
            return;
        }

        originalPos = cameraTransform.localPosition;
    }

    public static void Shake(float duration, float strength)
    {
        if (Instance == null || Instance.cameraTransform == null) return;
        Instance.StartCoroutine(Instance.ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 offset = Random.insideUnitCircle * strength;
            cameraTransform.localPosition = originalPos + (Vector3)offset;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalPos;
    }
}
