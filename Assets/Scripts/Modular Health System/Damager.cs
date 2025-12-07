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
    [Header("¿Quién usa este Damager? (Player / Enemy / Object)")]
    public TargetTypeEnum targetType = TargetTypeEnum.Player;

    [Header("Vibración al hacer daño")]
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.2f;

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
            // 🔥 Aplica el daño
            item.SetDamage(this);

            // 📌 Vibración universal
            // Si se hace daño a cualquier cosa con Damageable → vibración
            if (CameraShake.instance != null)
                CameraShake.instance.Shake(shakeDuration, shakeMagnitude);
        }
    }
}
