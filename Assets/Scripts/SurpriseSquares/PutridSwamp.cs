using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutridSwamp : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.poisoned= true;
    }


}
