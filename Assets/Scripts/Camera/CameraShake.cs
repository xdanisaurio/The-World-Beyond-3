using UnityEngine;
using System.Collections.Generic;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 basePosition;
    private List<ShakeData> activeShakes = new List<ShakeData>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        basePosition = transform.localPosition;
    }

    private void Update()
    {
        // Si el juego está pausado NO vibramos
        if (Time.timeScale == 0f)
        {
            ResetPosition();
            return;
        }

        Vector3 totalOffset = Vector3.zero;

        for (int i = activeShakes.Count - 1; i >= 0; i--)
        {
            ShakeData s = activeShakes[i];
            s.elapsed += Time.deltaTime;

            if (s.elapsed >= s.duration)
            {
                activeShakes.RemoveAt(i);
                continue;
            }

            float currentMag = s.magnitude * Random.Range(1f, 2f);
            totalOffset += Random.insideUnitSphere * currentMag;

            activeShakes[i] = s;
        }

        transform.localPosition = basePosition + totalOffset;
    }

    public void Shake(float duration, float magnitude)
    {
        activeShakes.Add(new ShakeData(duration, magnitude));
    }

    // --- NUEVO: Detener vibración ---
    public void StopShake()
    {
        activeShakes.Clear();
        ResetPosition();
    }

    private void ResetPosition()
    {
        transform.localPosition = basePosition;
    }

    private struct ShakeData
    {
        public float duration;
        public float magnitude;
        public float elapsed;

        public ShakeData(float d, float m)
        {
            duration = d;
            magnitude = m;
            elapsed = 0f;
        }
    }
}
