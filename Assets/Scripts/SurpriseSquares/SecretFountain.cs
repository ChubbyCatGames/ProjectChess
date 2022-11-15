using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretFountain : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.life = p.maxLife;
    }
}
