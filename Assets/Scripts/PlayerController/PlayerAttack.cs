using UnityEngine;


public class PlayerAttack : BasePlayerState
{

    public float speed = 8f;
    public float rotationSpeed = 10f;
    private float attackStartTime;
    private bool isAttacking = false;
    


    public PlayerAttack(CharacterController controller) : base(controller){}

    public override void EnterState()
    {
        isAttacking = true;
        attackStartTime = Time.time;
        
        controller.Rb.linearVelocity = Vector3.zero;//corregir
        controller.anim?.CrossFade("attack", 0.15f);
        controller.AttackCollider.SetActive(true);
        controller.StartAttackCoroutine();
    }

    public override void UpdateState()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();


        Vector3 right = Camera.main.transform.right;
        right.y = 0f;
        right.Normalize();

        
        


        Vector3 direction = (horizontal * right + vertical * forward).normalized;


        if (direction.magnitude > 0.1f)
        {

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.Player.rotation = Quaternion.Slerp(controller.Player.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }


        if (Time.time >= attackStartTime + controller.AttackDuration)
        {
            EndAttack();
        }

        if (controller.RechargeCount > 0f)
        {
            controller.RechargeCount -= Time.deltaTime;
        }
        

        
    }

    private void EndAttack()
    {
        if (!isAttacking) return;

        isAttacking = false;
        controller.AttackCollider.SetActive(false);
        controller.RechargeCount = controller.RechargeTime;
    }
    


    public bool CanExitState()
    {
        return !isAttacking && controller.RechargeCount <= 0f; //
        
    }


    public override void ExitState()
    {
        controller.AttackCollider.SetActive(false);
    }

}
