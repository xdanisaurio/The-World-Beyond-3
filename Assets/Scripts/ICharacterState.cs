using UnityEngine;

public interface ICharacterState
{
    void EnterState();
    void UpdateState();
    void FixedUpdate();
    void ExitState();
}
