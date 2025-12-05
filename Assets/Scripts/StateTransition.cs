using System;
using UnityEngine;

public class StateTransition
{
    private ICharacterState _newState;
    private Func<bool> _conditions;

    public ICharacterState NewState { get => _newState;}
    public Func<bool> Conditions { get => _conditions;}

    //Metodo contructor
    public StateTransition(ICharacterState newState, Func<bool> conditions)
    {
        _newState = newState; 
        _conditions = conditions;
    }
}
