using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public HealthSystem vidaEnemigo;
    public Slider healthSlider;

    [SerializeField] private float initialHealth;

    private Animator animator;
    private MachineStates machineStates;
    private EnemyDeathSound deathSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        machineStates = GetComponent<MachineStates>();
        deathSound = GetComponent<EnemyDeathSound>(); // <-- Añadido

        vidaEnemigo.Initialize(initialHealth);

        vidaEnemigo.HealthValueChanged += UpdateHealthSlider;
        vidaEnemigo.Death += OnDeath;

        healthSlider.maxValue = vidaEnemigo.maxHealth;
        healthSlider.value = vidaEnemigo.currentHealth;
    }

    private void OnDestroy()
    {
        vidaEnemigo.HealthValueChanged -= UpdateHealthSlider;
        vidaEnemigo.Death -= OnDeath;
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

        // Destruir después de animación (da tiempo para el audio)
        Destroy(gameObject, 3f);
    }
}