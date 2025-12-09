using UnityEngine;

public class ProjectileBoss : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 12f;
    public float destroyDistance = 0.5f;

    [Header("Impact Settings")]
    public GameObject impactEffect;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.05f;

    // ------------------------------------------------------
    //   SONIDOS
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
        // Si el proyectil NO tiene AudioSource, intenta tomar el del Boss.
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = FindFirstObjectByType<BossController>()?.GetComponent<AudioSource>();
        }

        // Reproducir sonido de disparo
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

        // Timer para permitir movimiento
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

        // Movimiento hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Cuando llega al destino
        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            // Efecto visual
            if (impactEffect != null)
            {
                var effect = Instantiate(impactEffect, targetPosition, Quaternion.identity);
                Destroy(effect, 2f);
            }

            // Reproducir sonido de impacto (NUEVO MÉTODO FIJO)
            PlaySoundAtPoint(impactSFX, targetPosition);

            // Shake de cámara
            if (CameraShake.instance != null)
                CameraShake.instance.Shake(shakeDuration, shakeMagnitude);

            // Destruir indicador
            if (indicatorInstance != null)
                Destroy(indicatorInstance);

            // Destruir proyectil
            Destroy(gameObject);
        }
    }

    // ------------------------------------------------------
    // 🟩 NUEVO MÉTODO: REPRODUCE EL SONIDO DE FORMA FIJA
    // ------------------------------------------------------
    private void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        GameObject soundObj = new GameObject("TempImpactSound");
        AudioSource a = soundObj.AddComponent<AudioSource>();

        a.clip = clip;
        a.volume = volume;
        a.spatialBlend = 1f;   // 3D
        soundObj.transform.position = position;

        a.Play();

        Destroy(soundObj, clip.length);
    }
}
