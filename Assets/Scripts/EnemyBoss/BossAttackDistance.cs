using UnityEngine;

public class BossAttackDistance : BaseBossState
{
    // ?? Sistema de sonido para este estado
    private AudioSource audioSource;
    public AudioClip shootSFX;

    public BossAttackDistance(BossController controller) : base(controller)
    {
        // Cargar o asignar el AudioSource desde el BossController
        audioSource = controller.GetComponent<AudioSource>();
    }

    public override void EnterState()
    {
        controller.AnimBoss?.CrossFade("SHOOT", 0.1f);
        Debug.Log("Entró en estado ATTACK DISTANCE");

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

    public override void FixedUpdate() { }

    public void DoShootProjectile()
    {
        // ? Aquí se reproduce el sonido de disparo
        if (shootSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSFX);
        }

        GameObject proj = Object.Instantiate(controller.distanceProjectile,
            controller.shootPivot.position,
            controller.shootPivot.rotation);

        proj.GetComponent<ProjectileBoss>().SetTarget(controller.player.position);
    }
}
