using UnityEngine;

public interface IUnitAnimationState
{
    UnitAnimationState UnitAnimationState { get; set; }
}

public enum UnitAnimationState
{
    Idle,
    Walk,
    Death
}