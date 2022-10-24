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
    
    //Variables that determinates the moves of every piece
    public bool hasMoved { get; private set; }
    public List<Vector2Int> avaliableMoves;

    private IObjectTweener tweener;

    public abstract List<Vector2Int> SelectAvaliableSquares();

    public abstract void InitializeValues();

    public abstract void PromoteFaith();

    public abstract void PromoteWar();

    public int life;
    public int attackDmg;
    public int richness;

    private void Awake()
    {
        avaliableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        materialSetter = GetComponent<MaterialSetter>();
        hasMoved = false;
        InitializeValues();
    }

    public void SetMaterial(Material mat)
    {
        materialSetter.SetSingleMaterial(mat);
    }

    internal bool IsAttackingPieceOFType<T>() where T : Piece
    {
        foreach(var square in avaliableMoves)
        {
            if (board.GetPieceOnSquare(square) is T)
            {
                return true;
            }
        }
        return false;
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
        Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
        occupiedSquare = coords;
        hasMoved = true;
        tweener.MoveTo(transform, targetPosition);
    }

    

    public void TryToAddMove(Vector2Int coords)
    {
        avaliableMoves.Add(coords);
    }

    //Gets called by the game controller and sets the attributes of the piece
    public void SetData(Vector2Int coords, PieceColor color, Board board)
    {
        this.color = color;
        occupiedSquare = coords;
        this.board = board;
        transform.position = board.CalculatePositionFromCoords(coords);
    }

    protected Piece GetPieceInDirection<T>(PieceColor color, Vector2Int direction) where T : Piece
    {
        for (int i = 1; i <= Board.BOARD_SIZE; i++)
        {
            Vector2Int nextCoords = occupiedSquare + direction * i;
            Piece piece = board.GetPieceOnSquare(nextCoords);
            if (!board.CheckIfCoordAreOnBoard(nextCoords))
                return null;
            if (piece != null)
            {
                if (piece.color != color || !(piece is T))
                    return null;
                else if (piece.color == color && piece is T)
                {
                    return piece;
                }
            }
        }
        return null;
    }

    internal void Attack(Piece defensor)
    {
        defensor.life -= attackDmg;
    }

    public string GetData()
    {
        return "Name: " + GetType().ToString() + "<br>Vida: " + life.ToString() + "<br>Atack: " + attackDmg.ToString();
    }

    //methods for in game interfacer

    public string GetName()
    {
        return GetType().ToString();
    }
    public string GetLife()
    {
        return life.ToString();
    }

    public string GetAttack()
    {
        return attackDmg.ToString();
    }
}
