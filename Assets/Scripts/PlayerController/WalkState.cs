using UnityEngine;
using UnityEngine.InputSystem.XR;


public class WalkState : BasePlayerState
{
    //public AnimatorController anim;
    public float speed = 8.5f;
    public float rotationSpeed = 10f;
    private Vector3 forward, right;
    public Vector3 direction1;
    


    public WalkState(CharacterController controller) : base(controller){}


    public override void EnterState()
    {
        
        controller.anim?.CrossFade("walk", 0.15f);
    }


    public override void UpdateState()
    {

        forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        right = Camera.main.transform.right;
        right.y = 0f;
        right.Normalize();

        //Recibimos las entradas del teclado con horizontal, vertical.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = (horizontal * right + vertical * forward).normalized;


        //Rotar suabe hacia la direcion que mira el personaje.
        if (direction.magnitude > 0.1f)
        {
            //Mover usando Rigidbody.
            Vector3 movement = direction * speed;
            movement.y = controller.Rb.linearVelocity.y;
            controller.Rb.linearVelocity = movement;
            
            //Rotar Suevemente hacia la direcion del movimiento.
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            controller.Player.transform.rotation = Quaternion.Slerp(
                controller.Player.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        else
        {
            //Detener movimiento si no hay entrada
            controller.Rb.linearVelocity = new Vector3(0f, controller.Rb.linearVelocity.y, 0f);
        }
    }



    public override void ExitState()
    {
        controller.anim.SetBool("IsWalking", false);//Desactivar animacion de caminar al salir.
    }


}
