using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Semi-Auto Decorator", menuName = "Weapon Decorators/Semi-Automatic")]
public class SemiDecorator : WeaponDecorator
{
    //public SemiDecorator(Weapon _owner) : base(_owner)
    //{
    //}

    public override void Disable()
    {
    }

    public override void Init(Weapon _owner)
    {
        base.Init(_owner);
    }
}
