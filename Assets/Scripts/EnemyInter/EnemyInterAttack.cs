using UnityEngine;


public class EnemyInterAttack : BaseEnemyInterState
{
    public float attackCooldown = 3f;
    private float lasAttackTime;
    private bool isBoomerangActive = false;//Para restrear que ya hay uno en vuelo

    public EnemyInterAttack(EnemyInterController controller) : base(controller) { }

    public override void EnterState()
    {
        controller.Agent.isStopped = true; // Se queda quieto al detectar al jugador.
        controller.Animator.CrossFade("ATTACK", 0.15f);
    }
    public override string ToString()
    {
        return "Attack";
    }

    public override void UpdateState()
    {
        //if (controller.Target != null)
        //{
        //    if (Time.time > lasAttackTime + attackCooldown && !isBoomerangActive)
        //    {
        //        controller.Animator.SetTrigger("ATTACK");
        //        lasAttackTime = Time.time;
        //        Debug.Log("Cross a ATTACK");
        //    }
        //}
    }

    public void LaunchBoomerangFromAnimation()
    {
        if (controller.BoomerangPrefab != null && !isBoomerangActive)
        {
            Debug.Log("Evento de animacion recibido! Lanzando boomerang...");
            Vector3 spawnPos = controller.BoomerangSpawnPoint.position;
            Quaternion spawnRot = controller.BoomerangSpawnPoint.rotation;


            GameObject boomerang = GameObject.Instantiate(controller.BoomerangPrefab,
                spawnPos, spawnRot);

            Boomerang boomerangScript = boomerang.GetComponent<Boomerang>();
            if (boomerangScript != null && controller.Target)
            {
                boomerangScript.Launch(controller.BoomerangSpawnPoint, controller.Target);
                boomerangScript.OnBoomerangReturn = () => isBoomerangActive = false;
                isBoomerangActive = true;
            }
        }
    }

}
