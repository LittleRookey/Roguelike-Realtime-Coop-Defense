using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : Ability
{
    [Header("Magic Missile settings")]
    private GameObject smallMagicCircle;
    
    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
    }
}
