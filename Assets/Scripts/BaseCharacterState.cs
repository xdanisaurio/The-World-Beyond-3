using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacterState : ICharacterState
{
    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdate() { }
    public virtual void ExitState() { }
}
