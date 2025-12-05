using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public HealthSystem vidaEnemigo;
    public Image healthFill;

    [SerializeField] private float initialHealth;

    private Animator animator;
    private MachineStates machineStates;
    private EnemyDeathSound deathSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        machineStates = GetComponent<MachineStates>();
        deathSound = GetComponent<EnemyDeathSound>();

        vidaEnemigo.Initialize(initialHealth);

        vidaEnemigo.HealthValueChanged += UpdateHealthFill;
        vidaEnemigo.Death += OnDeath;

        healthFill.fillAmount = vidaEnemigo.currentHealth / vidaEnemigo.maxHealth;
    }

    private void OnDestroy()
    {
        vidaEnemigo.HealthValueChanged -= UpdateHealthFill;
        vidaEnemigo.Death -= OnDeath;
    }

    private void UpdateHealthFill(float currentHealth)
    {
        // fillAmount va entre 0 y 1
        healthFill.fillAmount = currentHealth / vidaEnemigo.maxHealth;
    }

    private void OnDeath()
    {
        Debug.Log("El enemigo ha muerto");

        if (deathSound != null)
            deathSound.PlayDeathSound();

        if (animator != null)
            animator.SetTrigger("Death");

        if (machineStates != null)
            machineStates.enabled = false;

        if (BossEnemyManager.Instance != null)
        {
            BossEnemyManager.Instance.EnemigoMuerto(transform.position);
        }

        Destroy(gameObject, 3f);
    }
}