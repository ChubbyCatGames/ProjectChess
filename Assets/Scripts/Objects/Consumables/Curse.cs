using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : Consumable
{
    Curse()
    {
        cost = 400;
    }
    public override void OnUse(Piece p)
    {
        p.canMoveNextTurn = false;
    }

}
