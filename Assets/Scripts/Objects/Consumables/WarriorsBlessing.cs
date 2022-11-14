using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsBlessing : Consumable
{
    WarriorsBlessing()
    {
        cost = 800;
    }
    public override void OnUse(Piece p)
    {
        p.canMoveTwice = true;
    }

}
