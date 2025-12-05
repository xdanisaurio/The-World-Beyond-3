using UnityEngine;

public class EnemyInterPatrol : BaseEnemyInterState
{
    Transform currentPivot;

    //Contructor.
    public EnemyInterPatrol(EnemyInterController controller) : base(controller)
    {
    }
    public override void EnterState()
    {
        controller.Agent.isStopped = false;
        controller.Animator.CrossFade("RUN", 0.15f);
        ChooseRandomPivot();
    }
    public override string ToString()
    {
        return "Patrol";
    }
    public override void UpdateState()
    {

        if (controller.Agent.remainingDistance < 0.5f && !controller.Agent.pathPending)
        {
            ChooseRandomPivot();
        }
    }


    //Pivotos random.
    private void ChooseRandomPivot()
    {
        if (controller.Pivotes.Count > 0)
        {
            currentPivot = controller.Pivotes[Random.Range(0, controller.Pivotes.Count)];
            controller.Agent.SetDestination(currentPivot.position);
        }
    }
}
