using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public PieceColor team {  get; set; }
    public Board board { get; set; }
    public List<Piece> activePieces { get; set; }

    public Player(PieceColor team, Board board)
    {
        this.team = team;
        this.board = board;
        activePieces = new List<Piece>();
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
            if(board.HasPiece(piece))
                piece.SelectAvaliableSquares();
        }
    }
}
