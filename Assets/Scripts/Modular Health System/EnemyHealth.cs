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

    // ✅ Propiedad que indica si el enemigo está muerto
    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        machineStates = GetComponent<MachineStates>();
        deathSound = GetComponent<EnemyDeathSound>(); // <-- Añadido

        // Asegúrate de que vidaEnemigo no sea null en el inspector o inicialízalo aquí
        if (vidaEnemigo == null)
        {
            Debug.LogError($"[{name}] vidaEnemigo no está asignado en el inspector.");
            return;
        }

        vidaEnemigo.Initialize(initialHealth);

        vidaEnemigo.HealthValueChanged += OnHealthChanged;
        vidaEnemigo.Death += OnDeath;

        if (healthSlider != null)
        {
            healthSlider.maxValue = vidaEnemigo.maxHealth;
            healthSlider.value = vidaEnemigo.currentHealth;
        }

        lastHealth = vidaEnemigo.currentHealth;
    }

    private void OnDestroy()
    {
        if (vidaEnemigo != null)
        {
            vidaEnemigo.HealthValueChanged -= OnHealthChanged;
            vidaEnemigo.Death -= OnDeath;
        }
    }

    private void OnHealthChanged(float currentHealth)
    {
        if (healthSlider != null)
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
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    private void OnDeath()
    {
        // ✅ Marcar como muerto inmediatamente
        IsDead = true;

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

        // Evitar que se siga moviendo (si tiene Rigidbody)
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Si usas Unity 2020+ y Rigidbody tiene velocity:
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false; // opcional, según comportamiento deseado
        }

        //Notificar que este enemigo murió (para saber si es el último)
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.EnemigoMuerto(transform.position);

        // Destruir después de animación (da tiempo para el audio)
        Destroy(gameObject, 3f);
    }
}
