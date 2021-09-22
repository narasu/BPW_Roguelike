using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Automatic Decorator", menuName = "Weapon Decorators/Automatic")]
public class AutoDecorator : WeaponDecorator
{
    //public AutoDecorator(Weapon _owner) : base(_owner)
    //{
    //    _owner.automatic = true;
    //}

    public override void Init(Weapon _owner)
    {
        base.Init(_owner);
        owner.automatic = true;
    }

    public override void Disable()
    {
        owner.automatic = false;
    }

    public override void Shoot()
    {
    }
}
