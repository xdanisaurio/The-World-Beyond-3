using UnityEngine;

public class BossAttackBasic : BaseBossState
{
    // ?? Sistema de sonido para el ataque cuerpo a cuerpo
    private AudioSource audioSource;
    public AudioClip meleeAttackSFX;

    public BossAttackBasic(BossController controller) : base(controller)
    {
        // Tomamos el AudioSource desde el boss
        audioSource = controller.GetComponent<AudioSource>();
    }

    public override void EnterState()
    {
        Debug.Log("Entró al estado ATTACK");
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

    public override void FixedUpdate() { }

    public void DoBasicAttack()
    {
        // ? Sonido del ataque cuerpo a cuerpo
        if (meleeAttackSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(meleeAttackSFX);
        }

        controller.attackObject.SetActive(true);
        controller.StartCoroutine(DisableHitBoxShortly());
    }

    private System.Collections.IEnumerator DisableHitBoxShortly()
    {
        yield return new WaitForSeconds(0.0001f);
        controller.attackObject.SetActive(false);
    }
}
