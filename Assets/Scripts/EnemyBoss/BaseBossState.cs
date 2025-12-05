
public class BaseBossState : BaseCharacterState
{
    protected BossController controller;
    
    public BaseBossState (BossController controller)
    {
        this.controller = controller;
    }
}
