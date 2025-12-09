using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 12f;
    public float destroyDistance = 0.5f;

    [Header("Impact Settings")]
    public GameObject impactEffect;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.1f;

    // ------------------------------------------------------
    // ?? SONIDOS
    // ------------------------------------------------------
    private AudioSource audioSource;

    [Header("Sound Settings")]
    public AudioClip shootSFX;      // sonido al disparar
    public AudioClip impactSFX;     // sonido al impactar
    // ------------------------------------------------------

    // ---------------------------
    // INDICADOR PREVIO
    // ---------------------------
    [Header("Indicator Settings")]
    public GameObject indicatorPrefab;
    private GameObject indicatorInstance;
    public float indicatorDelay = -1f;
    private bool canMove = false;
    // ---------------------------


    private void Awake()
    {
        // si el proyectil no tiene audioSource, intenta tomarlo del boss
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = FindFirstObjectByType<BossController>()?.GetComponent<AudioSource>();
        }

        // reproducir sonido al disparar
        if (audioSource != null && shootSFX != null)
            audioSource.PlayOneShot(shootSFX);
    }


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

        // Movimiento del proyectil
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            // Spawnear efecto
            if (impactEffect != null)
            {
                var effect = Instantiate(impactEffect, targetPosition, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // SONIDO DE IMPACTO
            if (audioSource != null && impactSFX != null)
                audioSource.PlayOneShot(impactSFX);

            // Shake
            if (CameraShake.instance != null)
                CameraShake.instance.Shake(shakeDuration, shakeMagnitude);

            // Destruir indicador
            if (indicatorInstance != null)
                Destroy(indicatorInstance);

            Destroy(gameObject);
        }
    }
}
