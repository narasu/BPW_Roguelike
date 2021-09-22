using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDecorator : ScriptableObject
{
    protected Weapon owner;
    public virtual void Shoot()
    {

    }
    //public WeaponDecorator(Weapon _owner)
    //{
    //    owner = _owner;
    //}

    public virtual void Init(Weapon _owner)
    {
        owner = _owner;
    }
    public abstract void Disable();
}
