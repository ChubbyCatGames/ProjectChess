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
}
