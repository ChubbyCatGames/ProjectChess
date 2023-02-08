using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastMove : MonoBehaviour
{
    public Vector2Int newCoords;
    public Vector2Int oldCoords;
    public Piece piece;
    public Piece oldPiece;

    public LastMove()
    {
        newCoords = new Vector2Int();
        oldCoords = new Vector2Int();
        piece = null;
        oldPiece = null;
    }
}
