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
    [Header("¿Quién usa este Damager?")]
    public TargetTypeEnum targetType = TargetTypeEnum.Player;

    [Header("Vibración al hacer daño (solo si este Damager es del Player)")]
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
            // 🔥 Aplica daño
            item.SetDamage(this);

            // 📌 Vibración SOLO si:
            // - Este Damager pertenece al jugador
            // - El objeto golpeado es un enemigo
            if (targetType == TargetTypeEnum.Player && other.CompareTag("Enemy"))
            {
                if (CameraShake.instance != null)
                    CameraShake.instance.Shake(shakeDuration, shakeMagnitude);
            }
        }
    }
}
