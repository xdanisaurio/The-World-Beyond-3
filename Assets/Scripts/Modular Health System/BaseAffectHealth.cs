using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAffectHealth : MonoBehaviour, IAffectHealth
{
    public float Value { get => _value; set => _value = value; }
    [SerializeField] float _value;

    public List<TargetTypeEnum> ValidTargets { get => _validTargets; set => _validTargets = value; }
    [SerializeField] List<TargetTypeEnum> _validTargets;
}
