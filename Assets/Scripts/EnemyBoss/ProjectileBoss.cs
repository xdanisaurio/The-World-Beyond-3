using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 12f;
    public float destroyDistance = 0.5f;

    [Header("Impact Settings")]
    public GameObject impactEffect;
    public float shakeDuration = 1f;
    public float shakeMagnitude = 1f;

    // ---------------------------
    // INDICADOR PREVIO
    // ---------------------------
    [Header("Indicator Settings")]
    public GameObject indicatorPrefab;   // Prefab del círculo/partícula
    private GameObject indicatorInstance;
    public float indicatorDelay = -1f;    // Tiempo antes de que el proyectil se mueva
    private bool canMove = false;        // Bloquea movimiento
    // ---------------------------

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;

        // Crear indicador inmediatamente
        if (indicatorPrefab != null)
        {
            indicatorInstance = Instantiate(indicatorPrefab, targetPosition, Quaternion.identity);
        }

        // Comenzar timer para permitir movimiento
        if (indicatorDelay > 0)
            Invoke(nameof(EnableMovement), indicatorDelay);
        else
            canMove = true;
    }

    void EnableMovement()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
            return;

        // Movimiento
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            if (impactEffect != null)
            {
                var effect = Instantiate(impactEffect, targetPosition, Quaternion.identity);
                Destroy(effect, 2f);
            }

            if (CameraShake.instance != null)
                CameraShake.instance.Shake(shakeDuration, shakeMagnitude);

            // Destruir indicador al impactar
            if (indicatorInstance != null)
                Destroy(indicatorInstance);

            Destroy(gameObject);
        }
    }
}
