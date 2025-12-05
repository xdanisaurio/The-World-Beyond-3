using UnityEngine;

//Estado de persecucion del EnemigoBasico.
public class EnemyChase : BaseEnemyState
{
    

    //Constructor de la clase EnemyChase.
    public EnemyChase(EnemyController controller) : base(controller)
    {
    }


    //Metodo llamado de estado de persecucion.
    //En esta funcion permite el movimiento del Agent de navegacion.
    public override void EnterState()
    {
        controller.AnimBasic?.CrossFade("RUN", 0.15f);
        if (controller.Agent != null) 
            controller.Agent.isStopped = false;
       
    }


    //Metodo llamado cada frame mientras el enemigo esta en el estado de persecucion.
    //Y actualiza el Agent para que siga al personaje.
    public override void UpdateState()
    {
        //esto controla el seguimiento del agente a un target y su pocision(Personaje).
        if (controller.Target != null)
        {
            controller.Agent.SetDestination(controller.Target.position);
        }
    }

}
