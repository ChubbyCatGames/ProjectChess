using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
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
        this.maxLife = 60;
        this.life = this.maxLife;
        this.attackDmg = 60;
        this.richness = 40;
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

    public override void ChangeBranch()
    {
        board.PromotePieceWar(this, typeof(Pope));
    }

    public override bool CheckThreatNextTurn()
    {
        return false;
    }
    
    public override void PassiveAbility(Piece piece, Vector2Int coords)
    {
        List<Piece> pieces = board.GetPiecesOnPerpendicular(coords);
        foreach (var p in pieces)
        {
            if (!IsFromSameColor(p))
            {
                p.life -= 10*duplicatePassive;
            }
        }
        

    }
}
