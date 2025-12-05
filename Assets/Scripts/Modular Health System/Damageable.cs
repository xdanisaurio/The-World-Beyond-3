using System.Collections.Generic;
using UnityEngine;
public class Damageable : BaseHealthUtility
{
    public void SetDamage(IAffectHealth damager)
    {
        _healthSystem.SetDamage(damager);
    }
}
