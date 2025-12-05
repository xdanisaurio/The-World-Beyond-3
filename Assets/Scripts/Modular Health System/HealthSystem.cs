using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public delegate void HealthStatus();
    public delegate void HealthChanged(float currentHealth);
    public HealthStatus MaxHealthAchieved;
    public HealthStatus Death;
    public HealthChanged HealthValueChanged;

    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;

    [SerializeField] TargetTypeEnum objectType;
    public void Initialize(float initialHealth)
    {
        maxHealth = initialHealth;
        currentHealth = initialHealth;
        HealthValueChanged?.Invoke(currentHealth);
    }
    public void SetDamage(IAffectHealth damager)
    {
        if (!damager.ValidTargets.Contains(objectType))
            return;

        currentHealth -= damager.Value;
        HealthValueChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death?.Invoke();

            
            if (objectType != TargetTypeEnum.Player)
            {
                Destroy(gameObject, 2f); 

            }
        }
    }
    public void SetHeal(IAffectHealth heal)
    {
        if (!heal.ValidTargets.Contains(objectType))
            return;

        currentHealth += heal.Value;

        HealthValueChanged?.Invoke(currentHealth);

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            MaxHealthAchieved?.Invoke();
        }
    }
    public void SetTargetType(TargetTypeEnum type)
    {
        objectType = type;
    }
    public TargetTypeEnum GetTargetType()
    {
        return objectType;
    }
}
