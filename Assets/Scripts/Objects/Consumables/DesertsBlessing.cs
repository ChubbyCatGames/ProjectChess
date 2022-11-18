using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertsBlessing : Consumable
{
    DesertsBlessing()
    {
        cost = 250;
    }
    public override void OnUse(Piece p)
    {
        p.ChangeBranch();
    }

    private void Awake()
    {
        objectName = "Deserter's Blessing";
        objectDescription = "(Consumable) The unit can change the way it is being developed, from Faith to Army or vice versa";
        cost =250;
    }
}
