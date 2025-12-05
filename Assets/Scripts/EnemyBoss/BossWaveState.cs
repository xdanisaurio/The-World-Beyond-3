using Unity.VisualScripting;
using UnityEngine;

public class BossWaveState : BaseBossState
{
    
    private WaveManager waveManager;
    private bool waveStarted = false;


    public BossWaveState(BossController controller, WaveManager wm) : base(controller)
    {
        waveManager = wm;
    }

    public override void EnterState()
    {
        Debug.Log("Entro estado OLEADA");

        
        waveStarted = false;
        controller.AnimBoss?.CrossFade("INVOKE", 0.1f);
        if (controller.ColiderBoss != null)
        {
            controller.ColiderBoss.enabled = false;
        }
    }


    public override void UpdateState()
    {
        if (!waveStarted)
        {
            waveStarted = true;
            waveManager.StarWave();
        }
    }

    public override void ExitState()
    {

    }

    public void AnimationEvent_WaveFinished()
    {
        controller.MachineStates.SetState(controller.previousState);
        if (controller.ColiderBoss != null )
        {
            controller.ColiderBoss.enabled = true;
        }
        
    }
}
