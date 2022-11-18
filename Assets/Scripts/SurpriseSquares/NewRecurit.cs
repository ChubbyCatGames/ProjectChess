using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecurit : SquareEvent
{
    private Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down,
        new Vector2Int (-1,1),new Vector2Int (-1,-1),new Vector2Int(1,-1),new Vector2Int(1,1) };
    public override void StartEvent(Piece p)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (var dir in directions)
        {
            Vector2Int coords = p.occupiedSquare + dir;
            if (p.board.CheckIfCoordAreOnBoard(coords) && p.board.GetPieceOnSquare(coords)==null)
            {
                positions.Add(coords);
                p.board.newRecruitPositions.Add(coords);
            }
        }
        p.board.ShowSelectionSquares(positions);
        
        p.board.newRecruit = true;


    }

    private void Awake()
    {
        squareName = "New recruit";
        squareDescription = "A new pawn is generated in the chosen adjacent square";
    }
}
