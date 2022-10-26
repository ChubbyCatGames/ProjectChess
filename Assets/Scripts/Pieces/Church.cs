using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : Piece
{

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int (-1,1),
        new Vector2Int (-1,-1),
        new Vector2Int(1,-1),
        new Vector2Int(1,1),
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down

    };
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        float range = 2;
        foreach (var dir in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + dir * i;
                Piece piece = board.GetPieceOnSquare(nextCoords);
                if (!board.CheckIfCoordAreOnBoard(nextCoords))
                    break;
                if (piece == null)
                    TryToAddMove(nextCoords);
                else if (!piece.IsFromSameColor(this))
                {
                    TryToAddMove(nextCoords);
                    //NOTA DE DESARROLLO: AQUI AÑADIR FUNCION DE COMBATE
                    break;
                }
                else if (piece.IsFromSameColor(this))
                    break;
            }
        }
        return avaliableMoves;
    }

    public override void InitializeValues()
    {
        this.life = 65;
        this.attackDmg = 30;
        this.richness = 25;
        this.blessingDevelopCost = 6;
        this.goldDevelopCost = 220;
    }

    public override void PromoteFaith()
    {
        board.PromotePieceFaith(this, typeof(Pope));
    }

    public override void PromoteWar()
    {
        return;
    }
}
