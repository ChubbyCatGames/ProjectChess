using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pope : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int (-1,1),
        new Vector2Int (-1,-1),
        new Vector2Int(1,-1),
        new Vector2Int(1,1),
        

    };

    Vector2Int[] jumps = new Vector2Int[]
    {
        new Vector2Int(2,1),
        new Vector2Int(2,-1),
        new Vector2Int(-2,1),
        new Vector2Int(-2,-1),
        new Vector2Int(1,2),
        new Vector2Int(-1,2),
        new Vector2Int(1,-2),
        new Vector2Int(-1,-2),

    };

    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        for (int i = 0; i < jumps.Length; i++)
        {
            Vector2Int nextCoords = occupiedSquare + jumps[i];
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordAreOnBoard(nextCoords))
                continue;
            if (piece == null || !piece.IsFromSameColor(this))
                TryToAddMove(nextCoords);
        }
        float range = Board.BOARD_SIZE;
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

        this.maxLife = 80;
        this.life = this.maxLife;
        this.attackDmg = 40;
        this.richness = 35;
        this.blessingDevelopCost = 9999;
        this.goldDevelopCost = 9999;
    }

    public override void PromoteFaith()
    {
        return;
    }

    public override void PromoteWar()
    {
        return;
    }

    public override bool CheckThreatNextTurn()
    {
        return false;
    }
}
