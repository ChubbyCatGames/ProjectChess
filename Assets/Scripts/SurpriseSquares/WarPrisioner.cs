using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarPrisioner : SquareEvent
{
    public int cost;
    public override void StartEvent(Piece p)
    {
        //Iniciar ui al otro jugador

        p.ChangeTeam();
    }

    private void Awake()
    {
        squareName = "War prisoner";
        squareDescription = "The unit changes the side if the opponent can afford it, which costs 300 Gold";
    }
}
