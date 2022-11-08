using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
        new Vector2Int (-1,1),
        new Vector2Int (-1,-1),
        new Vector2Int(1,-1),
        new Vector2Int(1,1)


    };

    private Vector2Int[] directionsPawnThreatofBishop = new Vector2Int[]
    {
        new Vector2Int (-1,1),
        new Vector2Int (-1,-1),
        new Vector2Int(1,-1),
        new Vector2Int(1,1)
    };

    private Vector2Int[] jumpsPawnThreatofKinght = new Vector2Int[]
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

    private Vector2Int leftCastlingMove;
    private Vector2Int rightCastlingMove;

    private Piece leftRook;
    private Piece rightRook;
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        AssignStandardMoves();
        AssignCastlingMoves();
        return avaliableMoves;
    }

    private void AssignCastlingMoves()
    {
        if (hasMoved)
            return;
        leftRook = GetPieceInDirection<Rook>(color, Vector2Int.left);
        if(leftRook && !leftRook.hasMoved)
        {
            leftCastlingMove = occupiedSquare + Vector2Int.left * 2;
            avaliableMoves.Add(leftCastlingMove);
        }
        rightRook = GetPieceInDirection<Rook>(color, Vector2Int.right);
        if (rightRook && !rightRook.hasMoved)
        {
            rightCastlingMove = occupiedSquare + Vector2Int.right * 2;
            avaliableMoves.Add(rightCastlingMove);
        }
    }

    private void AssignStandardMoves()
    {
        float range = 1;
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
    }
    public override void MovePiece(Vector2Int coords)
    {
        base.MovePiece(coords);
        if (coords == leftCastlingMove)
        {
            board.UpdateBoardOnPieceMove(coords + Vector2Int.right, leftRook.occupiedSquare, leftRook, null);
            leftRook.MovePiece(coords + Vector2Int.right);
        }
        else if(coords == rightCastlingMove)
        {
            board.UpdateBoardOnPieceMove(coords+ Vector2Int.left, rightRook.occupiedSquare, rightRook, null);
            rightRook.MovePiece(coords+ Vector2Int.left);
        }
    }

    public override void InitializeValues()
    {
        this.maxLife = 10;
        this.life = this.maxLife;
        this.attackDmg = 999;
        this.richness = 0;
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
        PieceColor pc = this.color == PieceColor.White? PieceColor.Black : PieceColor.White;
        Piece p = null;
        for (int i = 0; i < 4; i++)
        {
            p = GetPieceInDirection<Knight>(pc, directions[i]);
            if (p != null) break;
        }
        if (!p)
        {
            for (int i = 0; i < directionsPawnThreatofBishop.Length; i++)
            {
                p = GetPieceInDirection<Pawn>(pc, directionsPawnThreatofBishop[i]);
                if (p != null) break;
            }
        }
        if (!p)
        {
            for (int i = 0; i < jumpsPawnThreatofKinght.Length; i++)
            {
                p = GetPieceInJumps<Pawn>(pc, jumpsPawnThreatofKinght[i]);
                if (p != null) break;
            }
        }
        if (!p)
            return false;
        Debug.Log("cuidado Caballo" + pc.ToString());
        
        return true;
    }
}
