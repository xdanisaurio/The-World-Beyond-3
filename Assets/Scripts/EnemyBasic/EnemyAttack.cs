using UnityEngine;

//Controlador de estado de ataque
//Estado que se activa cuando el personaje esta en el radio de alerta.
public class EnemyAttack : BaseEnemyState
{
    private bool isRotatingToTarget = false;
    private Quaternion targetRotation;
    private const float angleAttack = 6f;
    private bool _hasAttacked = false;

    public EnemyAttack(EnemyController controller) : base(controller) { }

    public override void EnterState()
    {
        // reproducir animación
        controller.AnimBasic?.CrossFade("ATTACK", 0.1f);

        controller.Agent.isStopped = true;
        controller.Agent.updateRotation = false;

        controller.SaveTargetPositionAttack();

        // rotación inicial hacia el jugador
        Vector3 dir = (controller.LastKnwonTargetPosition - controller.transform.position).normalized;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion  targetRotation = Quaternion.LookRotation(dir);
            controller.transform.rotation = targetRotation;
        }
        

        _hasAttacked = false;
    }

    public override void ExitState()
    {
        // asegurar que no quede el hitbox activado
        controller.SetAttackObject(false);
        controller.Agent.updateRotation = true;
    }

    public override void UpdateState()
    {
        if (controller.Target == null)
        {
            controller.MachineStates.SetState(controller.Patrol);
            return;
        }

        if (!controller.IsPlayerAlertRadius())
        {
            controller.MachineStates.SetState(controller.Chase);
            return;
        }

        // ROTACIÓN SUAVE
        if (!controller.IsAttackOnCooldown() && !_hasAttacked)
        {
            controller.AnimBasic.SetTrigger("Attack");
            controller.Agent.isStopped=true;
            controller.StarAttackCooldown();
            _hasAttacked=true;
        }

        else if (_hasAttacked && controller.IsAttackOnCooldown())
        {
            Vector3 directionToTarget = (controller.Target.position - controller.transform.position).normalized;
            directionToTarget.y = 0;

            if (directionToTarget.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation,
                    targetRotation, Time.deltaTime * controller.RotationSpeed);
            }
        }
        
            
        
        

        
        
    }
}
