using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public HealthSystem vidaEnemigo;
    public Slider healthSlider;

    [SerializeField] private float initialHealth;

    [Header("Damage Effect")]
    public ParticleSystem damageParticle; // <-- particula de daño
    public float particleOffsetY = 1f;

    private Animator animator;
    private MachineStates machineStates;
    private EnemyDeathSound deathSound;

    private float lastHealth;

    private void Start()
    {
        animator = GetComponent<Animator>();
        machineStates = GetComponent<MachineStates>();
        deathSound = GetComponent<EnemyDeathSound>(); // <-- Añadido

        vidaEnemigo.Initialize(initialHealth);

        vidaEnemigo.HealthValueChanged += OnHealthChanged;
        vidaEnemigo.Death += OnDeath;

        healthSlider.maxValue = vidaEnemigo.maxHealth;
        healthSlider.value = vidaEnemigo.currentHealth;

        lastHealth = vidaEnemigo.currentHealth;
    }

    private void OnDestroy()
    {
        vidaEnemigo.HealthValueChanged -= OnHealthChanged;
        vidaEnemigo.Death -= OnDeath;
    }

    private void OnHealthChanged(float currentHealth)
    {
        healthSlider.value = currentHealth;

        // Detectar si recibió daño
        if (currentHealth < lastHealth)
        {
            PlayDamageParticle();
        }

        lastHealth = currentHealth;
    }

    private void PlayDamageParticle()
    {
        if (damageParticle != null)
        {
            // Instanciar una partícula temporal
            ParticleSystem particle = Instantiate(
                damageParticle,
                transform.position + Vector3.up * particleOffsetY,
                Quaternion.identity
            );

            particle.Play();
            Destroy(particle.gameObject, particle.main.duration + particle.main.startLifetime.constantMax);
        }
    }

    private void UpdateHealthSlider(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    private void OnDeath()
    {
        Debug.Log("El enemigo ha muerto");

        // Reproducir sonido de muerte
        if (deathSound != null)
            deathSound.PlayDeathSound();

        // Animación de muerte
        if (animator != null)
            animator.SetTrigger("Death");

        // Desactivar IA
        if (machineStates != null)
            machineStates.enabled = false;

        // Evitar que se siga moviendo
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        //Notificar que este enemigo murió (para saber si es el último)
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.EnemigoMuerto(transform.position);


        // Destruir después de animación (da tiempo para el audio)
        Destroy(gameObject, 3f);
    }
}