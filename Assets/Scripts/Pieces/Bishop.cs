using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int (-1,1),
        new Vector2Int (-1,-1),
        new Vector2Int(1,-1),
        new Vector2Int(1,1),

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
        this.maxLife = 40;
        this.life = this.maxLife;
        this.attackDmg = 25;
        this.richness = 20;
        this.blessingDevelopCost = 3;
        this.goldDevelopCost = 140;
    }

    public override void PromoteFaith()
    {
        board.PromotePieceFaith(this, typeof(Church));
    }

    public override void PromoteWar()
    {
        return;
    }

<<<<<<< Updated upstream
    public override bool CheckThreatNextTurn()
    {
        return false;
=======
    public override void PassiveAbility(Piece piece, Vector2Int coords)
    {
        piece.life = piece.life* 1.2f;
>>>>>>> Stashed changes
    }
}
