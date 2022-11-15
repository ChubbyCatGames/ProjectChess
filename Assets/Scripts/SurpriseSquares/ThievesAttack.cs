using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThievesAttack : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.RemoveGold(p.richness * 2);
    }


}
