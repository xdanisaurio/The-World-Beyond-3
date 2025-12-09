using UnityEngine;

public class BossAttackBasic : BaseBossState
{
    // Sistema de sonido para el ataque cuerpo a cuerpo
    private AudioSource audioSource;
    public AudioClip meleeAttackSFX;

    public BossAttackBasic(BossController controller) : base(controller)
    {
        audioSource = controller.GetComponent<AudioSource>();
    }

    public override void EnterState()
    {
        Debug.Log("Entró al estado ATTACK");

        // --- NUEVO ---
        // Cuando entra a atacar, bloquea el cambio de estado
        controller.ataqueEnCurso = true;
        // --- NUEVO ---

        controller.AnimBoss?.CrossFade("ATTACK", 0.1f);
    }

    public override void UpdateState()
    {
        // --- CAMBIO ---
        // YA NO se sale del ataque aunque el jugador salga del radio
        // Solo se evaluará cuando termine la animación
        // --- CAMBIO ---
    }

    public override void ExitState()
    {
        controller.attackObject.SetActive(false);
    }

    public override void FixedUpdate() { }

    public void DoBasicAttack()
    {
        if (meleeAttackSFX != null && audioSource != null)
            audioSource.PlayOneShot(meleeAttackSFX);

        controller.attackObject.SetActive(true);
        controller.StartCoroutine(DisableHitBoxShortly());
    }

    private System.Collections.IEnumerator DisableHitBoxShortly()
    {
        yield return new WaitForSeconds(0.0001f);
        controller.attackObject.SetActive(false);
    }
}
