using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevoteeBlessing : Object
{
    DevoteeBlessing()
    {
        objectName = "Devotee's Blessing";
        objectDescription = "(Consumable) The Blessing cost for the next ascension of the unit is decreased by 1";
        cost = 400;
    }
    public override void OnUse(Piece p)
    {
        p.blessingDevelopCost -= 1;
    }
}
