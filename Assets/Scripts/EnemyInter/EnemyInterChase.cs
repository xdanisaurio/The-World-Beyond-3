using UnityEngine;


public class EnemyInterChase : BaseEnemyInterState
{
    public EnemyInterChase(EnemyInterController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.Agent.isStopped = false; // Se queda quieto al detectar al jugador.
        controller.Animator.CrossFade("RUN", 0.15f);
    }
    public override string ToString()
    {
        return "Chase";
    }

    public override void UpdateState()
    {
        if (controller.Target != null)
        {
            controller.Agent.SetDestination(controller.Target.position);
        }
    }
}
