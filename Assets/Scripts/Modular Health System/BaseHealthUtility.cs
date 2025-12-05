using UnityEngine;

public class BaseHealthUtility : MonoBehaviour, IHealthUtility
{
    public HealthSystem Health { get => _healthSystem; }
    [SerializeField] protected HealthSystem _healthSystem;
    private void Awake()
    {
        if (_healthSystem == null)
        {
            TryGetComponent(out _healthSystem);
        }
    }
    private void OnEnable()
    {
        _healthSystem.Death += OnDeathAction;
    }
    private void OnDisable()
    {
        _healthSystem.Death -= OnDeathAction;
    }
    public void Initialize(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
    }
    public void OnDeathAction()
    {
        Destroy(this);
    }
}
