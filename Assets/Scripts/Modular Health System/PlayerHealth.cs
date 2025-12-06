using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthSystem vidaPersonaje;
    [SerializeField] private float initialHealth;

    [Header("Damage Effect")]
    public ParticleSystem damageParticle; // <-- particula de daño
    public float particleOffsetY = 1f;

    private Animator animator;

    // ---- FEEDBACK DE DAÑO ----
    [Header("Feedback de Daño")]
    [SerializeField] private DamageFlashSimple damageFlash;
    [SerializeField] private DamageOverlay damageOverlay; // <= añadido al inspector
    [SerializeField] private AudioSource audioSourceDamage;
    [SerializeField] private AudioClip damageSound;

    private float lastHealth;

    private void Start()
    {
        if (vidaPersonaje != null)
        {
            vidaPersonaje.Initialize(initialHealth);

            // Detectar daño
            vidaPersonaje.HealthValueChanged += OnHealthChanged;

            // Detectar muerte
            vidaPersonaje.Death += OnPlayerDeath;


            // Guardar vida inicial para detectar daño
            lastHealth = vidaPersonaje.currentHealth;
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        // Detectar daño real
        if (currentHealth < lastHealth)
        {
            PlayDamageFeedback();
            PlayDamageParticle();
        }

        lastHealth = currentHealth;
    }

    private void PlayDamageFeedback()
    {
        // Flash rojo
        if (damageFlash != null)
            damageFlash.Flash();

        // Sonido de daño
        if (audioSourceDamage != null && damageSound != null)
            audioSourceDamage.PlayOneShot(damageSound);
    }

    private void PlayDamageParticle()
    {
        if (damageParticle != null)
        {
            // Instanciar partícula justo encima del jugador
            ParticleSystem particle = Instantiate(
                damageParticle,
                transform.position + Vector3.up * particleOffsetY,
                Quaternion.identity
            );

            particle.Play();
            Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constantMax);
        }
    }

    private void OnDamageTaken(float currentHealth)
    {
        // Flash en el personaje
        if (damageFlash != null)
            damageFlash.Flash();

        // Efecto en la pantalla (overlay rojo)
        if (damageOverlay != null)
            damageOverlay.Flash();

        // Sonido de daño
        if (audioSourceDamage != null && damageSound != null)
            audioSourceDamage.PlayOneShot(damageSound);
    }

    private void OnPlayerDeath()
    {
        Debug.Log("El jugador ha muerto");

        // ---- DESACTIVAR OVERLAY DE DAÑO AL MORIR ----
        if (damageOverlay != null)
            damageOverlay.gameObject.SetActive(false);

        if (GameManager.Instancia != null)
        {
            GameManager.Instancia.GameOver();
        }

        // Animación de muerte
        if (animator != null)
            animator.SetTrigger("Die");

        // Evitar movimiento al morir
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }

    private void OnDestroy()
    {
        if (vidaPersonaje != null)
        {
            vidaPersonaje.Death -= OnPlayerDeath;
            vidaPersonaje.HealthValueChanged -= OnDamageTaken;
        }
    }
}
