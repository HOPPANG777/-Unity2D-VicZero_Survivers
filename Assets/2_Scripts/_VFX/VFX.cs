using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum VFXType
{
    Peachone,
    ForceArea,
    LightningRing,
    RuneTracer,
    Nunchaku
}

public class VFX : MonoBehaviour
{
    public ParticleSystem PS;
    //public ParticleSystemRenderer PspriteRenderer;

    public virtual void Start()
    {
        PS = this.AddComponent<ParticleSystem>();
        //PspriteRenderer = PS.GetComponent<ParticleSystemRenderer>();
    }

}
