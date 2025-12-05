using System.Collections.Generic;
using UnityEngine;

public class MachineStates : MonoBehaviour
{
    public ICharacterState currentState;

    private Dictionary<ICharacterState, List<StateTransition>> transitions = new();
    private List<StateTransition> anyTransitions = new();

    [SerializeField] string currentStateName;
    public void AddTransition(ICharacterState state, StateTransition transition)
    {
        if (!transitions.ContainsKey(state))
        {
            transitions.Add(state, new List<StateTransition>());
        }
        transitions[state].Add(transition);
    }

    public void SetState(ICharacterState newState)
    {
        if (currentState != newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
            currentStateName = currentState?.ToString();
        }
    }

    private void Update()
    {
        currentState?.UpdateState();
        CheckAnyTransitions();
        CheckStateTransitions();
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    private void CheckAnyTransitions()
    {
        foreach (var item in anyTransitions)
        {
            if (item.Conditions())
            {
                SetState(item.NewState);
            }
        }
    }

    private void CheckStateTransitions()
    {
        if (currentState == null)return;

        if (transitions.ContainsKey(currentState))
        {
            foreach (var item in transitions[currentState])
            {
                if (item.Conditions())
                {
                    SetState(item.NewState);
                }
            }
        }
    }
}
