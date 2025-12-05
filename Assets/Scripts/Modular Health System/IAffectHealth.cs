using System.Collections.Generic;

public interface IAffectHealth
{
    float Value { get; }
    List<TargetTypeEnum> ValidTargets { get; }
}
