using System.Collections.Generic;
using UnityEngine;

public enum TargetTypeEnum
{
    Player,
    Enemy,
    Object
}

[RequireComponent(typeof(Collider))]
public class Damager : BaseAffectHealth
{
    Collider hitbox;
    private void Start()
    {
        TryGetComponent(out hitbox);
    }
    public void Enable()
    {
        hitbox.enabled = true;
    }
    public void Disable()
    {
        hitbox.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Damageable[] damageables = other.GetComponents<Damageable>();

        foreach (var item in damageables)
        {
            item.SetDamage(this);
        }
    }
}
