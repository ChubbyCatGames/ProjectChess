using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearsTrap : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.canMoveNextTurn = false;
    }


}
