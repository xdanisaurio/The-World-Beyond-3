using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthSystem vidaPersonaje;
    [SerializeField] private float initialHealth;

    private Animator animator;

    // ---- FEEDBACK DE DAÑO ----
    [Header("Feedback de Daño")]
    [SerializeField] private DamageFlashSimple damageFlash;
    [SerializeField] private DamageOverlay damageOverlay; // <= añadido al inspector
    [SerializeField] private AudioSource audioSourceDamage;
    [SerializeField] private AudioClip damageSound;

    private void Start()
    {
        if (vidaPersonaje != null)
        {
            vidaPersonaje.Initialize(initialHealth);

            // Detectar daño
            vidaPersonaje.HealthValueChanged += OnDamageTaken;

            // Detectar muerte
            vidaPersonaje.Death += OnPlayerDeath;
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
