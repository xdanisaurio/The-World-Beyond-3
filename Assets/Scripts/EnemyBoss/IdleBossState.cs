using UnityEngine;

public class IdleBossState : BaseBossState
{
    public IdleBossState(BossController controller) : base(controller) { }



    public override void EnterState()
    {
        Debug.Log("Entro al estado IDLE");
        controller.AnimBoss?.CrossFade("REPOSE",0.1f);
        controller.attackObject.SetActive(false);
    }


    public override void UpdateState()
    {
        if (controller.PlayerInRage())
        {
            controller.MachineStates.SetState(controller.attackBasicState);
        }
    }


    public override void ExitState(){}


    public override void FixedUpdate(){}


}
