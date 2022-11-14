using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public abstract bool CheckThreatNextTurn();

    public abstract void InitializeValues();

    public abstract void PromoteFaith();

    public abstract void PromoteWar();

    public abstract void ChangeBranch();

    private float m_life= 0;

    public float life
    {
        get { return m_life; }
        set
        {
            if (m_life == value) return;
            m_life = value;
            if (OnLifeChanged != null)
                OnLifeChanged();
        }
    }



    public float maxLife;
    public float attackDmg;
    public int richness;

    public int duplicatePassive = 1;

    public int blessingDevelopCost;
    public int goldDevelopCost;

    public Action OnLifeChanged;

    //variables needed to apply objects and passives
    public bool ignoreFirstAttack;
    public bool canMoveNextTurn;
    public bool canMoveTwice;

    public Object equipedObject;

    //this variable will make the piece die at the end of the turn
    public bool condemned;

    private void Awake()
    {
        avaliableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        materialSetter = GetComponent<MaterialSetter>();
        hasMoved = false;
        InitializeValues();
        OnLifeChanged += UpdateLifeUI;

        ignoreFirstAttack = false;
        canMoveNextTurn = true;
        canMoveTwice = false;
        condemned = false;
        equipedObject = null;

}

    private void OnDisable()
    {
        OnLifeChanged -= UpdateLifeUI;
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

    public virtual void PassiveAbility(Piece piece, Vector2Int coords)
    {
        return;
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
    protected Piece GetPieceInJumps<T>(PieceColor color, Vector2Int jump) where T : Piece
    {

        Vector2Int nextCoords = occupiedSquare + jump;
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
        
        return null;
    }

    internal void Attack(Piece defensor)
    {
        defensor.life -= attackDmg;
    }

    public void EquipObject(Object obj)
    {
        equipedObject= obj;
        OnEquip();
    }

    private void OnEquip()
    {
        equipedObject.OnUse(this);
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
        string cadena = life.ToString() + "/" + maxLife.ToString();
        return cadena;
    }

    public string GetAttack()
    {
        return attackDmg.ToString();
    }

    public string GetRichness()
    {
        return richness.ToString();
    }

    public string GetBlessingDevelopCost()
    {
        return blessingDevelopCost.ToString();
    }

    public string GetGoldDevelopCost()
    {
        return goldDevelopCost.ToString();
    }

    private void UpdateLifeUI()
    {
        if (life < maxLife)
        {
            //MOSTRAR DAÑO EN EL PREFAB DE LA PIEZA UN ICONO DE DAÑADO
            if(life <= 0)
            {
                board.TakePiece(this);
            }
            
        }
        else
        {
            //DESACTIVAR EL DAÑO
        }
    }
}
