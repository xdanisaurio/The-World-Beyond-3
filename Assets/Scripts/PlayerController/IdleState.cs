using UnityEngine;


public class IdleState : BasePlayerState
{


    public IdleState(CharacterController controller) : base(controller){}



    public override void EnterState()
    {
        
        controller.Rb.linearVelocity = new Vector3(0f, controller.Rb.linearVelocity.y, 0f);
        controller.anim?.CrossFade("idle", 0.15f);
    }
    
}
