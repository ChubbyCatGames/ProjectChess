using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsBlessing : Object
{
    WarriorsBlessing()
    {
        objectName = "Warrior's Blessing";
        objectDescription = "(Consumable) The selected unit will move twice only during this turn. (Take care, if the unit threats the enemy King with the first move, it won't be able to move the second time";
        cost = 800;
    }
    public override void OnUse(Piece p)
    {
        p.canMoveTwice = true;
    }

}
