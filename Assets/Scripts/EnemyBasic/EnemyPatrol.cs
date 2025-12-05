using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyPatrol : BaseEnemyState
{
    Transform currentPivot;

    //Contructor.
    public EnemyPatrol(EnemyController controller) : base(controller)
    {
    }
    public override void EnterState()
    {
        //controller.AnimBasic.SetTrigger("Run");
        ChooseRandomPivot();
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
