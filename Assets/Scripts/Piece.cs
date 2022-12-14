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
    public Board board { get; set; }

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

            //Check if the unit loses health
            if (value < m_life)
            {
                canvasDamage.GetComponentInChildren<DamageNumber>().DamageNumberAnimation(Mathf.FloorToInt(m_life - value));
            }
            if(value > maxLife)
            {
                value = maxLife;
            }
            //Set the value
            m_life = Mathf.CeilToInt(value);

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
    public bool poisoned;

    public bool promotedThisTurn=true;

    //Variable Canvas for the damage number
    [SerializeField] private Canvas canvasDamage;

    [SerializeField] private Canvas canvasHurt;

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
        poisoned = false;
        equipedObject = null;

        canvasHurt.enabled= false;

    }



    private void OnDisable()
    {
        OnLifeChanged -= UpdateLifeUI;
    }

    public void InitializeDamageCanvas()
    {
        if (this.color == PieceColor.White)
        {
            canvasDamage.GetComponent<RectTransform>().rotation = Quaternion.Euler(42.2f, 39.419f, 0);
        }
        else
        {
            canvasDamage.GetComponent<RectTransform>().rotation = Quaternion.Euler(42.2f, -140.581f, 0);
        }
        canvasDamage.GetComponent<RectTransform>().localPosition = new Vector3(7.2f, 169.8f, 4.4f);
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

        GameObject.Find("AudioManager").GetComponent<AudioManager>().movePiece.Play();
    }

    public virtual void PassiveAbility(Piece piece, Vector2Int coords)
    {
        return;
    }

    

    public void TryToAddMove(Vector2Int coords)
    {
        if(canMoveNextTurn)
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
        if(equipedObject!= null)
        {
            equipedObject.OnUnequip(this);
        }
        equipedObject= obj;
        OnEquip();
    }

    private void OnEquip()
    {
        equipedObject.OnUse(this);

        GameObject.Find("AudioManager").GetComponent<AudioManager>().equip.Play();
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
            //MOSTRAR DA?O EN EL PREFAB DE LA PIEZA UN ICONO DE DA?ADO
            if(life <= 0)
            {
                board.particleManager.PlayDeadParticles(board.CalculatePositionFromCoords(occupiedSquare));
                board.TakePiece(this);
            }
            canvasHurt.enabled= true;
        }
        else
        {
            //DESACTIVAR EL DA?O
            canvasHurt.enabled= false;
            if(life > maxLife)
            {
                life = maxLife;
            }

        }
    }
    public void ChangeTeam()
    {
        PieceColor newColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White; 

        board.ChangeTeamOfPiece(this, newColor);
    }
    internal void GetGold(int g)
    {
        board.particleManager.PlayWinGoldParticles(board.CalculatePositionFromCoords(occupiedSquare));
        board.AddGold(g);
    }

    internal void RemoveGold(int g)
    {
        board.particleManager.PlayLoseGoldParticles(board.CalculatePositionFromCoords(occupiedSquare));
        board.SubstractGold(g);
    }
}
