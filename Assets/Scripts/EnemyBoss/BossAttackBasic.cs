using UnityEngine;

public class BossAttackBasic : BaseBossState
{
    public BossAttackBasic(BossController controller) : base(controller){}


    public override void EnterState()
    {
        Debug.Log("Entro al estado ATTACK");
        controller.AnimBoss?.CrossFade("ATTACK", 0.1f);
    }


    public override void UpdateState()
    {
        if (!controller.PlayerInRage())
        {
            controller.MachineStates.SetState(controller.attackDistanceState);
        }
    }



    public override void ExitState()
    {
        controller.attackObject.SetActive(false);
    }



    public override void FixedUpdate(){}

    public void DoBasicAttack()
    {
        controller.attackObject.SetActive(true);


        controller.StartCoroutine(DisableHitBoxShortly());
    }

    private System.Collections.IEnumerator DisableHitBoxShortly()
    {
        yield return new WaitForSeconds(0.0001f);
        controller.attackObject.SetActive(false);
    }

    
   
}
