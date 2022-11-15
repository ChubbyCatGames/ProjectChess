using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.life /= 2;
    }

}
