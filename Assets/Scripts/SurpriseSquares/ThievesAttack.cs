using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThievesAttack : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.RemoveGold(p.richness * 2);
        
    }

    private void Awake()
    {
        squareName = "Thieves attack";
        squareDescription = "Lose as much Gold as the richness value of the piece multiplied by 2";
    }

}
