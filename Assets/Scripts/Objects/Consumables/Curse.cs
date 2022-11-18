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

    private void Awake()
    {
        objectName = "Curse";
        objectDescription = "(Consumable) An enemy unit selected won't be able to move in the next turn";
        cost =400;
    }
}
