using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Attribute : MonoBehaviour
{
    [SerializeField] float _baseValue; // 기본 속성 값
    public float Base_Value { get { return _baseValue; } set { _baseValue = value; } }

    [SerializeField] private float _modifiedValue;
    
    // 속성 값 가져오기
    public float GetValue()
    {
        return Base_Value + _modifiedValue;
    }

    // 속성 값 수정하기 (버프/디버프 적용)
    public void Modify(float amount)
    {
        _modifiedValue += amount;
    }
}
