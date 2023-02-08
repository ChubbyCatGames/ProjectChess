using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SquareSelectorCreator))]


public class Board : MonoBehaviour {
    public const int BOARD_SIZE = 8;
    
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    [SerializeField] public ParticleManager particleManager;


    public Piece[,] grid;
    private SquareEvent[,] gridEvents; 
    private Piece selectedPiece;
    public GameController controller;
    private SquareSelectorCreator squareSelector;

    public bool winSelectedPiece = true;

    public bool newRecruit = false;
    public List<Vector2Int> newRecruitPositions;

    [SerializeField]public UIManager uIManager;

    private Object itemSelected;

    private bool readyToPromote = false;

    public LastMove moves;
    private void Awake()
    {
        squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
        itemSelected = null;
        moves = new LastMove();
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
        if(!controller.IsGameInProgress() || !controller.IsPlayerTurn())
            return;
        Vector2Int coords= CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (itemSelected != null)
        {
            if (piece.GetType() != typeof(King))
            {
                piece.EquipObject(itemSelected);
                controller.activePlayer.RemoveObject(itemSelected);
                itemSelected = null;
                uIManager.UpdatePlayerItemsUI(controller.activePlayer);
                controller.SetNormalMouse();
            }
        }
        else if (selectedPiece)
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
                    CheckGridEvents(coords,GetPieceOnSquare(coords));
                }
            }
            if (piece != null && controller.IsTeamTurnActive(piece.color))
                SelectPiece(piece);

            
        }
    }

   

    public void SelectPiece(Piece piece)
    {
        if (readyToPromote) return;
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

            GameObject.Find("AudioManager").GetComponent<AudioManager>().selectPiece.Play();
        }
        else
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
        if (!controller.IsGameInProgress()) return;
        if (success)
        {
            UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
            selectedPiece.MovePiece(coords);
            DeselectPiece();
            if(piece.GetType() != typeof(King))
            {
                CheckGridEvents(coords, piece);
            }
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

    public void MovePieceAfterFight(Piece winner, Piece loser, Vector2Int coords )
    {

        UpdateBoardOnPieceMove(coords, winner.occupiedSquare, winner, null);
        winner.MovePiece(coords);
        particleManager.PlayDeadParticles(CalculatePositionFromCoords(coords));

        if (winner.GetType() != typeof(King))
        {
            CheckGridEvents(coords, winner);
        }
        if (winner.IsAttackingPieceOFType<King>())
            winner.canMoveTwice = false;
        if (!winner.canMoveTwice)
        {
            //EndTurn();

        }
        else
            winner.canMoveTwice = false;
        DeselectPiece();
        uIManager.inGameUi.SetActive(true);
    }
    public void MoveAuto(Piece p, Vector2Int coords)
    {
        UpdateBoardOnPieceMove(coords, p.occupiedSquare, p, null);
        p.MovePiece(coords);
        EndTurn();
        //controller.EndTurn();

        //particleManager.ChangeTurn();
        //itemSelected = null;
    }
    public void OnAIMove(Vector2Int coords, Piece piece)
    {
        selectedPiece = piece;
        bool success = TryToTake(coords);
        //if (!controller.IsGameInProgress()) return;
        if (success)
        {
            UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
            piece.MovePiece(coords);
            //DeselectPiece();
            if (piece.GetType() != typeof(King))
            {
                //CheckGridEvents(coords, piece);
            }
            if (piece.IsAttackingPieceOFType<King>())
                piece.canMoveTwice = false;

        }
        else
        {
            DeselectPiece();

        }
        controller.activePlayer.alreadyMoved = true;

        //EndTurn();
    }

    private void CheckGridEvents(Vector2Int coords, Piece piece)
    {
        if(gridEvents[coords.x, coords.y] != null)
        {
            //Set the ui info
            uIManager.UpdateSquareEventInfo(gridEvents[coords.x, coords.y].squareName, gridEvents[coords.x, coords.y].squareDescription);

            gridEvents[coords.x, coords.y].StartEvent(piece);
            DestroyEvent(gridEvents[coords.x, coords.y]);
            gridEvents[coords.x, coords.y]=null;

            if (uIManager.squareEventImg.GetComponent<InfoWindow>().WindowState == InfoWindow.State.Outside)
            {
                uIManager.CallSquareEventAnim();
            }
        }

    }

    public void CheckGridEventsAI(Vector2Int coords, Piece piece)
    {
        if (gridEvents[coords.x, coords.y] != null)
        {
            //Set the ui info
            uIManager.UpdateSquareEventInfo(gridEvents[coords.x, coords.y].squareName, gridEvents[coords.x, coords.y].squareDescription);
            gridEvents[coords.x, coords.y].StartEvent(piece);

            switch (gridEvents[coords.x, coords.y].squareName)
            {
                case "New recruit":
                    int rand = UnityEngine.Random.Range(0,newRecruitPositions.Count-1);
                    CreatePawnAI(newRecruitPositions[rand], PieceColor.Black);
                    break;
                case "Secret path":
                    
                    int i = UnityEngine.Random.Range(0, piece.avaliableMoves.Count - 1);
                    OnAIMove(piece.avaliableMoves[i],piece);
                    break;
                case "Rick, the merchant":
                    uIManager.BuyItemAI();
                    break;


            }


            DestroyEvent(gridEvents[coords.x, coords.y]);
            gridEvents[coords.x, coords.y] = null;

            if (uIManager.squareEventImg.GetComponent<InfoWindow>().WindowState == InfoWindow.State.Outside)
            {
                uIManager.CallSquareEventAnim();
            }
        }

    }

    //******ADD STATE FIGHT**********/////
    private bool TryToTake(Vector2Int coords)
    {

        
        Piece piece = GetPieceOnSquare(coords);
        //winSelectedPiece = true;

        if (piece != null && !selectedPiece.IsFromSameColor(piece))
        {
            controller.SetGameState(GameController.GameState.Fight);
            controller.StartFight(selectedPiece, piece, coords);

            //if (winSelectedPiece)
            //    TakePiece(piece);
            //else
            //    TakePiece(selectedPiece);
        }
        return true;
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
        //return if the windows are still doing the animations-----
        if (uIManager.whiteTurnImg.GetComponent<InfoWindow>().Animating || uIManager.blackTurnImg.GetComponent<InfoWindow>().Animating) return;
        //--------
        DeselectPiece();
        controller.EndTurn();
        particleManager.ChangeTurn();
        itemSelected = null;
    }

    //This method emulates the state of the board after a move is done
    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece piece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = piece;
        moves.newCoords = newCoords;
        moves.oldCoords = oldCoords;
        moves.piece = piece;
        moves.oldPiece = oldPiece;
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
    public List<Piece> GetPiecesOnAdjacent(Vector2Int coords)
    {
        List<Piece> pieces = new List<Piece>();
        Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down,
            new Vector2Int (-1,1),new Vector2Int (-1,-1),new Vector2Int(1,-1),new Vector2Int(1,1) };
        for (int i = 0; i < directions.Length; i++)
        {
            Piece p = GetPieceOnSquare(coords + directions[i]);
            if (p != null) pieces.Add(p);
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
            || selectedPiece.blessingDevelopCost > controller.activePlayer.blessing || selectedPiece.promotedThisTurn) return;
        controller.activePlayer.PieceDeveloped(selectedPiece);
        uIManager.ChangePlayerUI(controller.activePlayer,controller.activePlayer.team);
        particleManager.PlayLevelParticles(CalculatePositionFromCoords(selectedPiece.occupiedSquare));
        selectedPiece.PromoteFaith();

        GameObject.Find("AudioManager").GetComponent<AudioManager>().ascension.Play();
    }
    public void OnSelectedPiecePromoteWar()
    {
        //Check if is possible to develop a piece
        if (!selectedPiece || selectedPiece.goldDevelopCost > controller.activePlayer.gold
            || selectedPiece.blessingDevelopCost > controller.activePlayer.blessing || selectedPiece.promotedThisTurn) return;
        controller.activePlayer.PieceDeveloped(selectedPiece);
        uIManager.ChangePlayerUI(controller.activePlayer,controller.activePlayer.team);
        particleManager.PlayLevelParticles(CalculatePositionFromCoords(selectedPiece.occupiedSquare));
        selectedPiece.PromoteWar();

        GameObject.Find("AudioManager").GetComponent<AudioManager>().ascension.Play();
    }
    public void GetReadyToPromote()
    {
        readyToPromote = true;
        selectedPiece.avaliableMoves.Clear();
    }
    public void Promoted()
    {
        readyToPromote= false;
    }
    public void PromotePieceFaith(Piece p, Type t)
    {
        TakePiece(p);

        //Calculate the new life
        float ratio = p.life / p.maxLife;


        controller.CreatePieceAndInitializeHierLife(p.occupiedSquare, p.color, t, ratio);
        DeselectPiece();
    }

    public void PromotePieceWar(Piece p, Type t)
    {
        TakePiece(p);

        //Calculate the new life
        float ratio = p.life / p.maxLife;

        p.promotedThisTurn = true;
        controller.CreatePieceAndInitializeHierLife(p.occupiedSquare, p.color, t, ratio);
        DeselectPiece();
    }
    internal void ChangeTeamOfPiece(Piece piece, PieceColor newColor)
    {
        TakePiece(piece);

        //Calculate life ratio
        float ratio = piece.life / piece.maxLife;

        controller.CreatePieceAndInitializeHierLife(piece.occupiedSquare,newColor, piece.GetType(), ratio);
        DeselectPiece();
        controller.GenerateAllPossiblePlayerMoves(controller.activePlayer);
    }

    internal void CreatePawn(Vector2Int coords,PieceColor color)
    {
        controller.CreatePieceAndInitialize(coords, color, Type.GetType("Pawn"));
    }

    public void CreatePawnAI(Vector2Int coords,PieceColor color)
    {
        if (newRecruitPositions.Contains(coords))
        {

            controller.CreatePieceAndInitialize(coords, color, Type.GetType("Pawn"));
            newRecruit = false;
            DeselectPiece();
            newRecruitPositions.Clear();
            CheckGridEventsAI(coords, GetPieceOnSquare(coords));
        }
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

    public void SelectItem(Object item)
    {
        itemSelected = item;

    }
    public void AddGold(int gold)
    {
        controller.activePlayer.gold += gold;
    }

    public void SubstractGold(int gold)
    {
        controller.activePlayer.gold -= gold;
        uIManager.ChangePlayerUI(controller.activePlayer, controller.activePlayer.team);
    }

    public void OpenShop(List<GameObject> items)
    {
        uIManager.goldPlayer.text = controller.activePlayer.gold.ToString();
        uIManager.OpenShop(items);

        GameObject.Find("AudioManager").GetComponent<AudioManager>().store.Play();
    }

    public void TryToBuy(Object item)
    {
        controller.TryToBuy(item);
    }

    public void TryToBuyAI(Object item1,Object item2, Object item3) 
    {
        controller.TryToBuyAI(item1);
        if (uIManager.shopImg.activeSelf)
        {
            controller.TryToBuyAI(item2);
        }
        if (uIManager.shopImg.activeSelf)
        {
            controller.TryToBuyAI(item3);
        }
        uIManager.CloseShop();
    }

    Piece prisioner;
    internal void WarPrisioner(Piece p)
    {
        uIManager.WarPrisioner(p);
        prisioner = p;
    }

    public void WarPrisionerPaid()
    {
        if(controller.WarPrisionerPaid())
            prisioner.ChangeTeam();


    }

    internal void PieceDiedFighting(Piece piece)
    {
        particleManager.PlayDeadParticles(CalculatePositionFromCoords(piece.occupiedSquare));
        controller.activePlayer.alreadyMoved = true;
        DeselectPiece();
        uIManager.inGameUi.SetActive(true);
    }


}
