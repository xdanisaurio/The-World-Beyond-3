using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public HealthSystem vidaEnemigo;
    public Image healthFill;

    [SerializeField] private float initialHealth;

    [Header("Damage Effect")]
    public ParticleSystem damageParticle; // <-- particula de daño
    public float particleOffsetY = 2f;

    private Animator animator;
    private MachineStates machineStates;
    private EnemyDeathSound deathSound;

    private float lastHealth; // <-- para detectar daño real

    // -------------------------------------------------
    // 🔊 SONIDOS AÑADIDOS (esto es lo único agregado)
    // -------------------------------------------------
    [Header("Sonidos del Boss")]
    public AudioSource audioSource;   // arrastras el AudioSource del boss
    public AudioClip damageSFX;       // sonido cuando recibe daño
    public AudioClip deathSFX;        // sonido cuando muere
    // -------------------------------------------------

    private void Start()
    {
        animator = GetComponent<Animator>();
        machineStates = GetComponent<MachineStates>();
        deathSound = GetComponent<EnemyDeathSound>();

        vidaEnemigo.Initialize(initialHealth);

        vidaEnemigo.HealthValueChanged += OnHealthChanged;
        vidaEnemigo.Death += OnDeath;

        healthFill.fillAmount = vidaEnemigo.currentHealth / vidaEnemigo.maxHealth;

        lastHealth = vidaEnemigo.currentHealth;
    }

    private void OnDestroy()
    {
        vidaEnemigo.HealthValueChanged -= OnHealthChanged;
        vidaEnemigo.Death -= OnDeath;
    }

    private void OnHealthChanged(float currentHealth)
    {
        // Actualizar barra
        healthFill.fillAmount = currentHealth / vidaEnemigo.maxHealth;

        // Detectar daño
        if (currentHealth < lastHealth)
        {
            PlayDamageParticle();

            // 🔊 SONIDO DE DAÑO
            if (audioSource != null && damageSFX != null)
                audioSource.PlayOneShot(damageSFX);
        }

        lastHealth = currentHealth;
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

    private void OnDeath()
    {
        Debug.Log("El enemigo ha muerto");

        // 🔊 SONIDO DE MUERTE
        if (audioSource != null && deathSFX != null)
            audioSource.PlayOneShot(deathSFX);

        if (deathSound != null)
            deathSound.PlayDeathSound();

        if (animator != null)
            animator.SetTrigger("Death");

        if (machineStates != null)
            machineStates.enabled = false;

        if (BossEnemyManager.Instance != null)
            BossEnemyManager.Instance.EnemigoMuerto(transform.position);

        Destroy(gameObject, 3f);
    }
}
