using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IObjectTweener))]
[RequireComponent (typeof(MaterialSetter))]
public abstract class Piece : MonoBehaviour
{
    private MaterialSetter materialSetter;
    public Board board { protected get; set; }

    public Vector2Int occupiedSquare { get; set; }

    public PieceColor color { get; set; }
    public bool hasMoved { get; private set; }
    public List<Vector2Int> avaliableMoves;

    private IObjectTweener tweener;

    public abstract List<Vector2Int> SelectAvaliableSquares();

    private void Awake()
    {
        avaliableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        materialSetter = GetComponent<MaterialSetter>();
        hasMoved = false;
    }

    public void SetMaterial(Material mat)
    {
        materialSetter.SetSingleMaterial(mat);
    }

    public bool IsFromSameColor (Piece piece)
    {
        return color == piece.color;
    }
    
    public bool CanMoveTo(Vector2Int coords)
    {
        return avaliableMoves.Contains(coords);
    }

    public virtual void MovePiece(Vector2Int coords)
    {
        
    }

    public void TryToAddMove(Vector2Int coords)
    {
        avaliableMoves.Add(coords);
    }
    public void SetData(Vector2Int coords, PieceColor color, Board board)
    {
        this.color = color;
        occupiedSquare = coords;
        this.board = board;
        transform.position = board.CalculatePositionFromCoords(coords);
    }
}
