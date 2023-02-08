using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretPath : SquareEvent
{
    public int cost = 200;
    private Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right };
    public override void StartEvent(Piece p)
    {
        //if(paga)
        float range = Board.BOARD_SIZE;
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (var dir in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = p.occupiedSquare + dir * i;
                Piece piece = p.board.GetPieceOnSquare(nextCoords);
                if (!p.board.CheckIfCoordAreOnBoard(nextCoords))
                    break;
                if (piece == null)
                {
                    p.TryToAddMove(nextCoords);
                    positions.Add(nextCoords);
                }
                else
                {
                    break;
                }
            }
        }
        p.board.SelectPiece(p);
        p.board.ShowSelectionSquares(positions);
    }

    private void Awake()
    {
        squareName = "Secret path";
        squareDescription = "The unit can be moved to the chosen square in the same row. However, this has a cost of 200 Gold";
    }
}
