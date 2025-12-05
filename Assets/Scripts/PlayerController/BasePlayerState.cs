
public class BasePlayerState : BaseCharacterState
{
    protected CharacterController controller;
    public BasePlayerState(CharacterController controller)
    {
        this.controller = controller;
    }

    public override void EnterState(){}
    public override void UpdateState(){}
    public override void ExitState(){}
}
