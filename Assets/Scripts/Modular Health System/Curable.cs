using UnityEngine;

public class Curable : BaseHealthUtility
{
    public void SetHeal(IAffectHealth heal)
    {
        _healthSystem.SetHeal(heal);
    }
}
