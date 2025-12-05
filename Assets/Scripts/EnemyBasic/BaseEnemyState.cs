

public abstract class BaseEnemyState : BaseCharacterState
{
    protected EnemyController controller;
    public BaseEnemyState(EnemyController controller)
    {
        this.controller = controller;
    }
}
