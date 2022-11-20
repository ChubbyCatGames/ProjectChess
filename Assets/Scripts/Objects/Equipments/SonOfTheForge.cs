using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonOfTheForge : Object
{

    SonOfTheForge()
    {
        objectName = "Daughter of the forge";
        objectDescription = "(Equipment) Grants 10 attack damage to the unit";
        cost = 350;
    }

    public override void OnUse(Piece p)
    {
        p.attackDmg+= 10;
    }
    public override void OnUnequip(Piece p)
    {
        p.attackDmg -= 10;
    }


}
