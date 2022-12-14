using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientTreasure : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.GetGold(p.richness * 2);
    }

    private void Awake()
    {
        squareName = "Ancient Treasure";
        squareDescription = "Grants as much Gold as the richness value of the piece multiplied by 2";
    }
}
