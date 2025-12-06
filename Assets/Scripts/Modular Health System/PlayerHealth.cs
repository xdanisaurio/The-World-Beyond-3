using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthSystem vidaPersonaje;
    [SerializeField] private float initialHealth;

    [Header("Damage Effect")]
    public ParticleSystem damageParticle;
    public float particleOffsetY = 1f;

    private Animator animator;

    // ---- FEEDBACK DE DAÑO ----
    [Header("Feedback de Daño")]
    [SerializeField] private DamageFlashSimple damageFlash;
    [SerializeField] private DamageOverlay damageOverlay;
    [SerializeField] private AudioSource audioSourceDamage;
    [SerializeField] private AudioClip damageSound;

    private float lastHealth;

    private void Start()
    {
        if (vidaPersonaje != null)
        {
            vidaPersonaje.Initialize(initialHealth);

            vidaPersonaje.HealthValueChanged += OnHealthChanged;
            vidaPersonaje.Death += OnPlayerDeath;

            lastHealth = vidaPersonaje.currentHealth;
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        if (currentHealth < lastHealth)
        {
            PlayDamageFeedback();
            PlayDamageParticle();
        }

        lastHealth = currentHealth;
    }

    private void PlayDamageFeedback()
    {
        // Flash rojo del personaje
        if (damageFlash != null)
            damageFlash.Flash();

        // Overlay rojo en pantalla
        if (damageOverlay != null)
            damageOverlay.Flash();

        // Sonido de daño
        if (audioSourceDamage != null && damageSound != null)
            audioSourceDamage.PlayOneShot(damageSound);
    }

    private void PlayDamageParticle()
    {
        if (damageParticle != null)
        {
            ParticleSystem particle = Instantiate(
                damageParticle,
                transform.position + Vector3.up * particleOffsetY,
                Quaternion.identity
            );

            particle.Play();
            Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constantMax);
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("El jugador ha muerto");

        // Quitar overlay para que no tape el panel de muerte
        if (damageOverlay != null)
            damageOverlay.gameObject.SetActive(false);

        if (GameManager.Instancia != null)
            GameManager.Instancia.GameOver();

        if (animator != null)
            animator.SetTrigger("Die");

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }

    private void OnDestroy()
    {
        if (vidaPersonaje != null)
        {
            vidaPersonaje.Death -= OnPlayerDeath;
            vidaPersonaje.HealthValueChanged -= OnHealthChanged;
        }
    }
}

