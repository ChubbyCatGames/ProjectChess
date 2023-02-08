using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        Vector2Int direction = color == PieceColor.White ? Vector2Int.up : Vector2Int.down;
        float range = hasMoved ? 1 : 2;
        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordAreOnBoard(nextCoords))
                break;
            if (piece == null)
                TryToAddMove(nextCoords);
            else
                break;
        }

        Vector2Int [] takeDirections = new Vector2Int[] { new Vector2Int(1,direction.y) , new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirections.Length; i++)
        {
            Vector2Int nextCoords = occupiedSquare + takeDirections[i];
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (piece != null && !piece.IsFromSameColor(this) && board.CheckIfCoordAreOnBoard(nextCoords))
                TryToAddMove(nextCoords);
                    //NOTA DE DESARROLLO: AQUI SE AÑADE EL CODIGO DEL COMBATE
        }
        return avaliableMoves;
    }

    public override void InitializeValues()
    {

        this.maxLife = 35;
        this.life= this.maxLife;
        this.attackDmg = 20;
        this.richness = 10;
        this.blessingDevelopCost = 2;
        this.goldDevelopCost = 75;
        this.value = 100f;

        gridValues = new float[,]{ {0, 5, 5, 0, 5, 10, 50, 0 },
                                    {0, 10, -5, 0, 5, 10, 50, 0},
                                    {0, 10, -10, 0, 10, 20, 50, 0},
                                    {0, -20, 0, 20, 25, 30, 50, 0},
                                    {0, -20, 0, 20, 25, 30, 50, 0},
                                    {0, 10, -10, 0, 10, 20, 50, 0},
                                    {0, 10, -5, 0, 5, 10, 50, 0},
                                    {0, 5, 5, 0, 5, 10, 50, 0}};
    }

    public override void PromoteFaith()
    {
        board.PromotePieceFaith(this, typeof(Bishop));
    }

    public override void PromoteWar()
    {
        board.PromotePieceWar(this, typeof(Knight));
    }

    public override void ChangeBranch()
    {
        return;
    }
    public override bool CheckThreatNextTurn()
    {
        return false;
    }
}
