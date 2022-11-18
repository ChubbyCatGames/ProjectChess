using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonOfTheForge : Equipment
{

    SonOfTheForge()
    {
        cost = 350;
        dmgAddition = 10;
    }

    public override void OnUse(Piece p)
    {
        p.attackDmg+= dmgAddition;
    }
    public override void OnUnequip(Piece p)
    {
        p.attackDmg -= dmgAddition;
    }

    private void Awake()
    {
        objectName = "Daughter of the forge";
        objectDescription = "(Equipment) Grants 10 attack damage to the unit";
        cost =350;
    }
}
