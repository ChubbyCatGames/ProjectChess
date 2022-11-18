using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player 
{
    public PieceColor team {  get; set; }
    public Board board { get; set; }
    public List<Piece> activePieces { get; set; }
    public int gold { get; set; }
    public int blessing { get; set; }
    public bool alreadyMoved = false;

    public Player(PieceColor team, Board board)
    {
        this.team = team;
        this.board = board;
        activePieces = new List<Piece>();
        gold = 0;
        blessing = 0;
    }

    public void AddPiece(Piece p)
    {
        if (!activePieces.Contains(p))
            activePieces.Add(p);
    }

    public void RemovePiece(Piece p)
    {
        if(activePieces.Contains(p))
            activePieces.Remove(p);
    }

    public void GenerateAllPosibleMoves()
    {
        foreach (var piece in activePieces)
        {
            if (board.HasPiece(piece) && piece.canMoveNextTurn)
                piece.SelectAvaliableSquares();
            else
            {
                piece.avaliableMoves.Clear();
                piece.canMoveNextTurn= true;
            }

        }
    }

    public Piece[] GetPieceAttakingPieceOfType<T>() where T : Piece
    {
        return activePieces.Where(p => p.IsAttackingPieceOFType<T>()).ToArray();
    }

    public Piece [] GetPiecesOfType<T>()where T: Piece
    {
        return activePieces.Where(p => p is T).ToArray();
    }

    public Piece[] GetTheratNextMove<T>() where T : Piece
    {
        return activePieces.Where(p => p.CheckThreatNextTurn()).ToArray();
    }

    internal void RemoveMovesEnablingAttackOnPiece<T>(Player opponent, Piece selectedPiece) where T : Piece
    {
        List<Vector2Int> coordsToRemove = new List<Vector2Int>();
        foreach (var coords in selectedPiece.avaliableMoves)
        {
            Piece pieceOnSquare = board.GetPieceOnSquare(coords);
            board.UpdateBoardOnPieceMove(coords, selectedPiece.occupiedSquare, selectedPiece, null);
            opponent.GenerateAllPosibleMoves();
            if(opponent.CheckIfIsAttackingPiece<T>())
                coordsToRemove.Add(coords);
            board.UpdateBoardOnPieceMove(selectedPiece.occupiedSquare,coords, selectedPiece, pieceOnSquare);
        }
        foreach (var coords in coordsToRemove)
        {
            selectedPiece.avaliableMoves.Remove(coords);
        }
    }

    private bool CheckIfIsAttackingPiece<T>() where T : Piece
    {
        foreach (var piece in activePieces)
        {
            if(board.HasPiece(piece) && piece.IsAttackingPieceOFType<T>())
            {
                return true;
            }
        }
        return false;
    }

    public bool CanHidePieceFromAttack<T>(Player opponent) where T : Piece
    {
        foreach (var piece in activePieces)
        {
            foreach(var coords in piece.avaliableMoves)
            {
                Piece pieceOnCoords = board.GetPieceOnSquare(coords);
                board.UpdateBoardOnPieceMove(coords,piece.occupiedSquare, piece, null);
                opponent.GenerateAllPosibleMoves();
                if (!opponent.CheckIfIsAttackingPiece<T>())
                {
                    board.UpdateBoardOnPieceMove(piece.occupiedSquare, coords, piece, pieceOnCoords);
                    return true;
                }
                board.UpdateBoardOnPieceMove(piece.occupiedSquare, coords, piece, pieceOnCoords);
            }
        }
        return false;
    }

    public void OnGameRestarted()
    {
        activePieces.Clear();
        gold = 0;
        blessing= 0;
    }

    public void UpdateGold()
    {
        foreach (var piece in activePieces)
        {
            gold += piece.richness;
        }
    }

    public void PieceDeveloped(Piece piece)
    {
        gold -= piece.goldDevelopCost;
        blessing -= piece.blessingDevelopCost;
    }
}
