using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    private Vector2Int [] directions = new Vector2Int[] {Vector2Int.left,Vector2Int.right,Vector2Int.up,Vector2Int.down};
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        float range = Board.BOARD_SIZE;
        foreach (var dir in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords= occupiedSquare + dir*i;
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
        this.maxLife = 45;
        this.life = this.maxLife;
        this.attackDmg = 50;
        this.richness = 30;
        this.blessingDevelopCost = 5;
        this.goldDevelopCost = 300;
    }

    public override void PromoteFaith()
    {
        return;
    }

    public override void PromoteWar()
    {
        board.PromotePieceWar(this, typeof(Queen));
    }

    public override bool CheckThreatNextTurn()
    {
        return false;
    }
}
