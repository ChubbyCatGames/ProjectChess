using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearsTrap : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.canMoveNextTurn = false;
    }

    private void Awake()
    {
        squareName = "Bear Trap";
        squareDescription = "The unit is locked and can't move during the next turn";
    }

}
