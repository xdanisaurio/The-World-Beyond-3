using System.Collections.Generic;
using UnityEngine;

public class Heal : BaseAffectHealth
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Curable curable))
        {
            curable.SetHeal(this);
        }
    }
}
