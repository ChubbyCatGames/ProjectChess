using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;
    
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    [SerializeField] public ParticleManager particleManager;


    private Piece[,] grid;
    private SquareEvent[,] gridEvents; 
    private Piece selectedPiece;
    private GameController controller;
    private SquareSelectorCreator squareSelector;

    public bool winSelectedPiece = true;

    public bool newRecruit = false;
    public List<Vector2Int> newRecruitPositions;

    [SerializeField] UIManager uIManager;

    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependencies(GameController controller)
    {
        this.controller = controller;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
        gridEvents = new SquareEvent[BOARD_SIZE, BOARD_SIZE];
    }

    //Uses the position of the bottom left square to apply the starting position
    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position+ new Vector3(coords.x*squareSize,0f,coords.y*squareSize);
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x/squareSize)+BOARD_SIZE/2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / squareSize) + BOARD_SIZE / 2;
        return new Vector2Int(x,y);
    }

    public void onSquareSelected(Vector3 inputPosition)
    {
        if(!controller.IsGameInProgress())
            return;
        Vector2Int coords= CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && selectedPiece != piece && controller.IsTeamTurnActive(piece.color))
                SelectPiece(piece);
            else if (selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, selectedPiece);
        }
        else
        {
            if (newRecruit)
            {
                if (newRecruitPositions.Contains(coords))
                {
                    CreatePawn(coords, controller.activePlayer.team);
                    newRecruit = false;
                    DeselectPiece();
                    newRecruitPositions.Clear();
                }
            }
            if (piece != null && controller.IsTeamTurnActive(piece.color))
                SelectPiece(piece);
        }
    }

   

    public void SelectPiece(Piece piece)
    {
        DeselectPiece();
        controller.RemoveMovesEnablingAttackOnPieceOfType<King>(piece);
        selectedPiece = piece;
        if(selectedPiece.canMoveTwice || !controller.activePlayer.alreadyMoved)
        {
            List<Vector2Int> selection = selectedPiece.avaliableMoves;
            ShowSelectionSquares(selection);
            uIManager.UpdateUI();

            //Call selection animation
            piece.GetComponent<SelectAnimation>().StartSelectAnimation(1.4f);
        }else
            DeselectPiece();
    }

    public void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 pos = CalculatePositionFromCoords(selection[i]);
            bool freeSquare = GetPieceOnSquare(selection[i])==null;
            squaresData.Add(pos, freeSquare);
        }
        squareSelector.ShowSelection(squaresData);
    }

    internal void SetPieceOnBoard(Vector2Int coords, Piece newPiece)
    {
        if(CheckIfCoordAreOnBoard(coords))
            grid[coords.x,coords.y] = newPiece;
    }

    internal void SetEventOnBoard(Vector2Int coords, SquareEvent newEvent)
    {
        if (CheckIfCoordAreOnBoard(coords))
            gridEvents[coords.x, coords.y] = Instantiate(newEvent);
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
        uIManager.UpdateUI();
    }

    private void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        bool success=TryToTake(coords);
        
        if (success)
        {
            UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
            selectedPiece.MovePiece(coords);
            DeselectPiece();
            CheckGridEvents(coords, piece);
            if (piece.IsAttackingPieceOFType<King>())
                piece.canMoveTwice = false;
            if (!piece.canMoveTwice)
            {
                //EndTurn();

            }
            else
                piece.canMoveTwice= false;
        }
        else
        {
            DeselectPiece();
            if (!piece.canMoveTwice)
            {
                //EndTurn();
            }
            else
                piece.canMoveTwice = false;
        }
        controller.activePlayer.alreadyMoved = true;
    }

    private void CheckGridEvents(Vector2Int coords, Piece piece)
    {
        Debug.Log(gridEvents[coords.x, coords.y]);
        if(gridEvents[coords.x, coords.y] != null)
        {
            gridEvents[coords.x, coords.y].StartEvent(piece);
            DestroyEvent(gridEvents[coords.x, coords.y]);
            gridEvents[coords.x, coords.y]=null;
            

        }

    }

    //******ADD STATE FIGHT**********/////
    private bool TryToTake(Vector2Int coords)
    {

        
        Piece piece = GetPieceOnSquare(coords);
        winSelectedPiece = true;

        if (piece != null && !selectedPiece.IsFromSameColor(piece))
        {
            controller.StartFight(selectedPiece, piece, coords);
            if (winSelectedPiece)
                TakePiece(piece);
            else
                TakePiece(selectedPiece);
        }
        return winSelectedPiece;
    }

    public void TakePiece(Piece piece)
    {
        if (piece)
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            controller.OnPieceRemoved(piece);
        }
    }

    public void DestroyEvent(SquareEvent _event)
    {
        if (_event)
        {
            Destroy(_event.gameObject);
        }
    }

    public void EndTurn()
    {
        controller.EndTurn();
        particleManager.ChangeTurn();
    }

    //This method emulates the state of the board after a move is done
    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece piece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = piece;
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }

    public List<Piece> GetPiecesOnPerpendicular(Vector2Int coords)
    {
        List <Piece> pieces = new List<Piece>();
        Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        for (int i = 0; i < directions.Length; i++)
        {
            Piece p = GetPieceOnSquare(coords + directions[i]);
            if(p!=null) pieces.Add(p);
        }
        return pieces;
    }

    public bool CheckIfCoordAreOnBoard(Vector2Int coords)
    {
        if(coords.x < 0 || coords.y < 0 || coords.x>= BOARD_SIZE || coords.y>=BOARD_SIZE)
            return false;
        return true;
    }

    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if(grid[i,j] == piece) { return true; }
            }
        }
        return false;
    }

    public void OnSelectedPiecePromoteFaith()
    {
        //Check if is possible to develop a piece
        if (!selectedPiece || selectedPiece.goldDevelopCost > controller.activePlayer.gold 
            || selectedPiece.blessingDevelopCost > controller.activePlayer.blessing) return;
        controller.activePlayer.PieceDeveloped(selectedPiece);
        uIManager.ChangePlayerUI(controller.activePlayer);
        particleManager.PlayLevelParticles(CalculatePositionFromCoords(selectedPiece.occupiedSquare));
        selectedPiece.PromoteFaith();
    }
    public void OnSelectedPiecePromoteWar()
    {
        //Check if is possible to develop a piece
        if (!selectedPiece || selectedPiece.goldDevelopCost > controller.activePlayer.gold
            || selectedPiece.blessingDevelopCost > controller.activePlayer.blessing) return;
        controller.activePlayer.PieceDeveloped(selectedPiece);
        uIManager.ChangePlayerUI(controller.activePlayer);
        particleManager.PlayLevelParticles(CalculatePositionFromCoords(selectedPiece.occupiedSquare));
        selectedPiece.PromoteWar();
    }

    public void PromotePieceFaith(Piece p, Type t)
    {
        TakePiece(p);
        controller.CreatePieceAndInitializeHierLife(p.occupiedSquare, p.color, t, p.life);
        DeselectPiece();
        controller.GenerateAllPossiblePlayerMoves(controller.activePlayer);
    }

    public void PromotePieceWar(Piece p, Type t)
    {

        TakePiece(p);
        controller.CreatePieceAndInitializeHierLife(p.occupiedSquare, p.color, t, p.life);
        DeselectPiece();
        controller.GenerateAllPossiblePlayerMoves(controller.activePlayer);
    }
    internal void ChangeTeamOfPiece(Piece piece, PieceColor newColor)
    {
        TakePiece(piece);
        controller.CreatePieceAndInitializeHierLife(piece.occupiedSquare,newColor, piece.GetType(), piece.life);
        DeselectPiece();
        controller.GenerateAllPossiblePlayerMoves(controller.activePlayer);
    }

    internal void CreatePawn(Vector2Int coords,PieceColor color)
    {
        controller.CreatePieceAndInitialize(coords, color, Type.GetType("Pawn"));
    }

    public void OnGameRestarted()
    {
        DeselectPiece();
        CreateGrid();
    }

    public Piece getSelectedPiece()
    {
        return selectedPiece;
    }

    public void AddGold(int gold)
    {
        controller.activePlayer.gold += gold;
    }

    public void SubstractGold(int gold)
    {
        controller.activePlayer.gold -= gold;
    }

    public void OpenShop(List<GameObject> items)
    {
        uIManager.OpenShop(items);
    }

    public void TryToBuy(Object item)
    {
        controller.TryToBuy(item);
    }
}
