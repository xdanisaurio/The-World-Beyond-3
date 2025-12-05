using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public HealthSystem vidaJugador;
    public Image barraVida;

    private void Start()
    {
        vidaJugador.HealthValueChanged += UpdateHealtFill;
        vidaJugador.Death += OnDeath;

        barraVida.fillAmount = vidaJugador.currentHealth / vidaJugador.maxHealth;
    }

    private void OnDestroy()
    {
        vidaJugador.HealthValueChanged -= UpdateHealtFill;
        vidaJugador.Death -= OnDeath;
    }

    private void UpdateHealtFill(float currentHealth)
    {
        barraVida.fillAmount = currentHealth / vidaJugador.maxHealth;
    }

    private void OnDeath()
    {
        Debug.Log("murió");
    }
}
