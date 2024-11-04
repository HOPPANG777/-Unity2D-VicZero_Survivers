using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum VFX_Type
{
    Peachone,
    Force_Area,
    LightningRing,
    RuneTracer,
    Nunchaku
}

public class VFX : MonoBehaviour
{
    public ParticleSystem PS;
    //public ParticleSystemRenderer P_spriteRenderer;

    public virtual void Start()
    {
        PS = this.AddComponent<ParticleSystem>();
        //P_spriteRenderer = PS.GetComponent<ParticleSystemRenderer>();
    }

}
