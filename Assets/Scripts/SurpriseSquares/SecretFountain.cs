using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretFountain : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.life = p.maxLife;
    }


    private void Awake()
    {
        squareName = "Hidden fountain";
        squareDescription = "The unit restores all of his health";
    }
}
