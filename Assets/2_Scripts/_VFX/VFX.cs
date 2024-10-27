using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum VFX_Type
{
    Peachone,
    Garlic_Area,
    LightningRing,
    RuneTracer,
    Whip
}

public class VFX : MonoBehaviour
{
    public ParticleSystem PS;
    //public ParticleSystemRenderer PSR;

    public virtual void Start()
    {
        PS = this.AddComponent<ParticleSystem>();
        //PSR = PS.GetComponent<ParticleSystemRenderer>();
    }

}
