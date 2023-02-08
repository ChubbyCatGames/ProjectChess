using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBoard : MonoBehaviour
{
    public Piece[,] grid = new Piece[8, 8];

    public LastMove moves = new LastMove();


    public VirtualBoard GenerateVirtualBoard(Piece p, Vector2Int coords)
    {
        VirtualBoard aux = new VirtualBoard();
        aux.CopyGrid(grid, aux.grid);
        aux.UpdateBoardOnPieceMove(aux.grid,coords, p.occupiedSquare, p, null);
        //aux.GetPieceOnSquare(coords).occupiedSquare = coords;
        return aux;

    }
    public void UpdateBoardOnPieceMove(Piece[,] grid, Vector2Int newCoords, Vector2Int oldCoords, Piece piece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = piece;
        moves.newCoords = newCoords;
        moves.oldCoords = oldCoords;
        moves.piece = piece;
        moves.oldPiece = oldPiece;
    }

    public void CopyGrid(Piece[,]gridToCopy, Piece[,]gridCopied)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                gridCopied[i, j] = gridToCopy[i, j];
            }
        }
    }
}
