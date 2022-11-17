using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutridSwamp : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.poisoned= true;
    }


    private void Awake()
    {
        squareName = "Putrid swamp";
        squareDescription = "The unit gets poisoned and loses 5 health points each turn";
    }
}
