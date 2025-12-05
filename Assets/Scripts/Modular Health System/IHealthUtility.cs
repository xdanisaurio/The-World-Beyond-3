using UnityEngine;

public interface IHealthUtility
{
    HealthSystem Health { get; }
    public TargetTypeEnum GetTargetType()
    {
        return Health.GetTargetType();
    }
}
