using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
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
            if(piece == null || !piece.IsFromSameColor(this))
                TryToAddMove(nextCoords);
        }
        return avaliableMoves;
    }

    public override void InitializeValues()
    {
        this.maxLife = 35;
        this.life = this.maxLife;
        this.attackDmg = 30;
        this.richness = 20;
        this.blessingDevelopCost = 3;
        this.goldDevelopCost = 140;
    }

    public override void PromoteFaith()
    {
        return;
    }

    public override void PromoteWar()
    {
        board.PromotePieceWar(this, typeof(Rook));
    }

    public override bool CheckThreatNextTurn()
    {
        return false;
    }
}
