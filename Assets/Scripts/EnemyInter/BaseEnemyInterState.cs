using UnityEngine;

public abstract class BaseEnemyInterState : BaseCharacterState
{
    protected EnemyInterController controller;

    public BaseEnemyInterState(EnemyInterController controller)
    {
        this.controller = controller;
    }
}
