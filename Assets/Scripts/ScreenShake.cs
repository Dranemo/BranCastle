using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration = 1f;
    private bool isShaking = false;
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = GetComponent<CameraFollow>();
    }

    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        float time = 0;
        Vector3 originalPos = transform.localPosition;

        while (time < duration && gameObject != null)
        {
            time += Time.deltaTime;
            float strength = curve.Evaluate(time / duration);
            transform.localPosition = originalPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }
}
