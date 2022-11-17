using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecurit : SquareEvent
{
    private Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
    public override void StartEvent(Piece p)
    {
        Vector2Int coords = p.occupiedSquare + directions[0];

        p.board.CreatePawn(coords, p.color);
    }

    private void Awake()
    {
        squareName = "New recruit";
        squareDescription = "A new pawn is generated in the chosen adjacent square";
    }
}
