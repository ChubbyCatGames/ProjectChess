using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertsBlessing : Object
{
    DesertsBlessing()
    {
        objectName = "Deserter's Blessing";
        objectDescription = "(Consumable) The unit can change the way it is being developed, from Faith to Army or vice versa";
        cost = 250;
    }
    public override void OnUse(Piece p)
    {
        p.ChangeBranch();
    }


}
