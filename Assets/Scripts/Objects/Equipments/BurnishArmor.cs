using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurnishArmor : Object
{
    BurnishArmor()
    {
        objectName = "Burnish armor";
        objectDescription = "(Equipment) Grants 15 maximum health to the unit";
        cost = 300;

    }


    public override void OnUse(Piece p)
    {
        p.life += 15;
    }
    public override void OnUnequip(Piece p)
    {
        p.life -= 15;
    }
}
