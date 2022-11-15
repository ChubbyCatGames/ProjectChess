using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientTreasure : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.GetGold(p.richness * 2);
    }
}
