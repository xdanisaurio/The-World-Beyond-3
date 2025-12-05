using UnityEngine;

public class BossAttackDistance : BaseBossState
{
    public BossAttackDistance(BossController controller) : base(controller){}



    public override void EnterState()
    {
        controller.AnimBoss?.CrossFade("SHOOT", 0.1f);
        Debug.Log("Entro en estado ATTACK DISTANCE");
        controller.LaserLine.gameObject.SetActive(true);
    }



    public override void UpdateState()
    {
        if (controller.PlayerInRage())
        {
            controller.MachineStates.SetState(controller.attackBasicState);
        }
    }

    public override void ExitState()
    {
        controller.LaserLine.gameObject.SetActive(false);
    }
    
    
    public override void FixedUpdate(){}


    public void DoShootProjectile()
    {
        GameObject proj = Object.Instantiate(controller.distanceProjectile,
            controller.shootPivot.position, controller.shootPivot.rotation);

        proj.GetComponent<ProjectileBoss>().SetTarget(controller.player.position);
    }







}
