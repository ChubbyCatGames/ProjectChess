using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun.Demo.Cockpit;

[RequireComponent(typeof(PieceCreator))]
public class GameController : MonoBehaviour
{
    public enum GameState { Init, Play, Finished, Fight}

    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private BoardLayoutEvents startingBoardEvents;
    [SerializeField] private Board board;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private MouseManager mouseManager;

    private PieceCreator pieceCreator;
    private CameraManager cameraManager;

    //Instances of the players and the active player
    private Player whitePlayer;
    private Player blackPlayer;
    public Player activePlayer;

    public GameState gameState;




    private void Awake()
    {
        SetDependencies();
        CreatePLayers();
    }

    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
        cameraManager = GetComponent<CameraManager>();
    }

    private void CreatePLayers()
    {
        whitePlayer = new Player(PieceColor.White, board);
        blackPlayer = new Player(PieceColor.Black, board);
    }
    void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        SetGameState(GameState.Init);
        board.SetDependencies(this);
        CreatePiecesFromLayout(startingBoardLayout);
        CreateEventsFromLayout(startingBoardEvents);
        activePlayer = whitePlayer;

        //Animation for the turn window
        uiManager.CallFirstTurnWindow();

        activePlayer.alreadyMoved= false;
        GenerateAllPossiblePlayerMoves(activePlayer);
        SetGameState(GameState.Play);
        GetTitheAndBlessing();
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state; 
    }

    public bool IsGameInProgress()
    {
        return gameState == GameState.Play;
    }


    //Takes the layout from unity and places the pieces down
    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords= layout.GetSquareCoordsAtIndex(i);
            PieceColor pieceColor= layout.GetSquarePieceColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);

            Type type= Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, pieceColor, type);
        }
    }

    private void CreateEventsFromLayout(BoardLayoutEvents layout)
    {
        layout.ShuffleEvents();
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {


            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            SquareEvent _event = layout.GetSquareEventNameAtIndex(i);

            _event.transform.position = board.CalculatePositionFromCoords(squareCoords);
            board.SetEventOnBoard(squareCoords, _event);
            
        }
    }



    //Gets called from above and initializes every piece
    public void CreatePieceAndInitialize(Vector2Int squareCoords, PieceColor pieceColor, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, pieceColor, board);

        newPiece.InitializeDamageCanvas();

        if (pieceColor == PieceColor.White)
        {
            newPiece.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Material colorMaterial = pieceCreator.GetPieceMaterial(pieceColor, newPiece.GetType());
        newPiece.SetMaterial(colorMaterial);

        board.SetPieceOnBoard(squareCoords, newPiece);

        Player currentPlayer = pieceColor == PieceColor.White ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(newPiece);

    }

    public void CreatePieceAndInitializeHierLife(Vector2Int squareCoords, PieceColor pieceColor, Type type, float lifeRatio)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, pieceColor, board);
        newPiece.life = lifeRatio * newPiece.maxLife;

        Material colorMaterial = pieceCreator.GetPieceMaterial(pieceColor, newPiece.GetType());
        newPiece.SetMaterial(colorMaterial);

        board.SetPieceOnBoard(squareCoords, newPiece);

        Player currentPlayer = pieceColor == PieceColor.White ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(newPiece);

    }

    public void GenerateAllPossiblePlayerMoves(Player player)
    {
        player.GenerateAllPosibleMoves();
    }

    public bool IsTeamTurnActive(PieceColor color)
    {
        return activePlayer.team == color;
    }

    internal void EndTurn()
    {
        TakeAllPiecesWithoutLife(activePlayer);
        TakeAllPiecesWithoutLife(GetOpponentToPlayer(activePlayer));
        GenerateAllPossiblePlayerMoves(activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(activePlayer));

        if (CheckIfGameIsFinished())
        {
            bool whiteWinner = activePlayer.team == PieceColor.White;
            EndGame(whiteWinner);
        }
        else
            ChangeActiveTeam();
        

    }

    private void TakeAllPiecesWithoutLife(Player p)
    {
        foreach (var piece in p.activePieces)
        {
            if (piece.poisoned) piece.life -= 3;
            if (piece.life <= 0 || piece.condemned) board.TakePiece(piece);
                
        }
    }
    private bool CheckIfGameIsFinished()
    {
        Piece[] kingAttackingPieces = activePlayer.GetPieceAttakingPieceOfType<King>();
        bool kingAlive = GetOpponentToPlayer(activePlayer).GetPiecesOfType<King>().Any();
        if (!kingAlive)
        {
            return true;
        }
        if(kingAttackingPieces.Length > 0)
        {
            Player oppositePlayer = GetOpponentToPlayer(activePlayer);
            Piece attackedKing = oppositePlayer.GetPiecesOfType<King>().FirstOrDefault();
            oppositePlayer.RemoveMovesEnablingAttackOnPiece<King>(activePlayer, attackedKing);

            int availableKingMoves = attackedKing.avaliableMoves.Count;
            if (availableKingMoves == 0)
            {
                bool canCoverKing = oppositePlayer.CanHidePieceFromAttack<King>(activePlayer);
                if (!canCoverKing)
                    return true;
            }
        }
        return false;
    }

    public void StartFight(Piece attacker, Piece defensor, Vector2Int coords)
    {

        GameObject.Find("AudioManager").GetComponent<AudioManager>().duel.Play();

        //Start a coroutine to make an animation

        int hitsAtck = 0;
        int hitsDef = 0;

        List<Piece> attackerChurches = new List<Piece>();
        List<Piece> defensorChurches = new List<Piece>();
        List<Piece> attackerKnights = new List<Piece>();
        List<Piece> defensorKnights = new List<Piece>();

        List<Piece> piecesPerpendicular = board.GetPiecesOnPerpendicular(coords);
        foreach(Piece piece in piecesPerpendicular) 
        {
            if (piece.IsFromSameColor(attacker))
            {
                if (piece.GetType() == typeof(Church))
                {
                    attackerChurches.Add(piece);
                }
                else if (piece.GetType() == typeof(Knight))
                {
                    attackerKnights.Add(piece);
                }
                else
                {
                    piece.PassiveAbility(attacker, coords);
                }

                
            }
            else if (piece.IsFromSameColor(defensor))
            {
                if (piece.GetType() == typeof(Church))
                {
                    defensorChurches.Add(piece);
                }
                else if (piece.GetType() == typeof(Knight))
                {
                    defensorKnights.Add(piece);
                }
                else
                {
                    piece.PassiveAbility(defensor, coords);
                }
            }

           
        }

        
        while (attacker.life>0 && defensor.life > 0)
        {

            if (defensor.ignoreFirstAttack)
            {
                defensor.ignoreFirstAttack = false;

                GameObject.Find("AudioManager").GetComponent<AudioManager>().divineShield.Play();
            }
            else
            {
                attacker.Attack(defensor);
                hitsAtck++;
                
            }
            for (int i = 0; i < attackerChurches.Count; i++)
            {
                attackerChurches[i].PassiveAbility(attacker, coords);
            }


            if (defensor.life > 0)
            {
                if (attacker.ignoreFirstAttack)
                {
                    attacker.ignoreFirstAttack = false;

                    GameObject.Find("AudioManager").GetComponent<AudioManager>().divineShield.Play();
                }
                else
                {
                    defensor.Attack(attacker);
                    hitsDef++;
                   
                    //defensordedasd
                }
                for (int i = 0; i < defensorChurches.Count; i++)
                {
                    defensorChurches[i].PassiveAbility(defensor, coords);
                }
            }
        }
        StartCoroutine(uiManager.StartFightUI(attacker, defensor,hitsAtck,hitsDef,coords));
       

        if (defensor.life <= 0)
        { 
            //board.winSelectedPiece = true;
            //uiManager.StopFight();
            Debug.Log(activePlayer.gold);
            float extraGold = 0;
            foreach(Knight k in attackerKnights.Cast<Knight>())
            {
                extraGold += k.goldAddition;
            }
            activePlayer.gold += defensor.richness + Mathf.FloorToInt(attacker.richness * extraGold);
            Debug.Log(activePlayer.gold);

        }
        else
        {
            //board.winSelectedPiece = false;
            //uiManager.StopFight();
            float extraGold = 0;
            foreach (Knight k in defensorKnights)
            {
                extraGold += k.goldAddition;
            }
            GetOpponentToPlayer(activePlayer).gold += attacker.richness + Mathf.FloorToInt(attacker.richness * extraGold);
        }
          
    }

    private void EndGame(bool whiteWon)
    {
        SetGameState(GameState.Finished);

        uiManager.EndGameUI(whiteWon);
    }


    public void OnPieceRemoved(Piece piece)
    {
        Player pieceOwner = (piece.color == PieceColor.White) ? whitePlayer : blackPlayer;
        pieceOwner.RemovePiece(piece);
        Destroy(piece.gameObject);
        
    }

    private Player GetOpponentToPlayer(Player player)
    {
        return player == whitePlayer ? blackPlayer : whitePlayer; 
    }

    private void ChangeActiveTeam()
    {
        Piece [] churches=  activePlayer.GetPiecesOfType<Church>();
        foreach(Church church in churches)
        {
            church.HealAdjacent(church.occupiedSquare);
        }
        
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer;

        //---Call the turn window---

        uiManager.CallTurnWindow(activePlayer == whitePlayer ? true : false);

        //--------------------
        
        activePlayer.GetTheratNextMove<King>();
        activePlayer.alreadyMoved = false;
        //uiManager.UpdatePlayerItemsUI(activePlayer);
        activePlayer.EnableAllPieces();
        GetTitheAndBlessing();

        GameObject.Find("AudioManager").GetComponent<AudioManager>().turnChange.Play();
    }

    private void GetTitheAndBlessing()
    {
        activePlayer.UpdateGold();
        activePlayer.blessing += 1;
        uiManager.ChangePlayerUI(activePlayer,activePlayer.team);
    }

    public void RemoveMovesEnablingAttackOnPieceOfType<T>(Piece p) where T : Piece
    {
        activePlayer.RemoveMovesEnablingAttackOnPiece<T>(GetOpponentToPlayer(activePlayer), p);
        
    }

    public void RestartGame()
    {
        DestroyPieces();
        board.OnGameRestarted();
        whitePlayer.OnGameRestarted();
        blackPlayer.OnGameRestarted();
        StartNewGame();
    }

    private void DestroyPieces()
    {
        whitePlayer.activePieces.ForEach(p => Destroy(p.gameObject));
        blackPlayer.activePieces.ForEach(p => Destroy(p.gameObject));
    }

    public void TryToBuy(Object item)
    {
        if(activePlayer.gold >= item.cost)
        {
            activePlayer.AddObject(item);

            board.SubstractGold(item.cost);
            uiManager.UpdatePlayerItemsUI(activePlayer);
            uiManager.CloseShop();

            GameObject.Find("AudioManager").GetComponent<AudioManager>().spentMoney.Play();
        }
        else
        {
            uiManager.NotEnoughGold();
        }
        
    }

    public void SelectItemAtIndex(int idx)
    {
        board.SelectItem(activePlayer.playerObjects[idx]);
        Cursor.SetCursor(mouseManager.mouseObject, Vector2.zero, CursorMode.ForceSoftware);
        
    }

    public void setNormalMouse()
    {
        Cursor.SetCursor(mouseManager.mouseText, new Vector2(1, 2), CursorMode.ForceSoftware);

    }

    internal bool WarPrisionerPaid()
    {

        int goldPlayer = GetOpponentToPlayer(activePlayer).gold;
        if(goldPlayer >= 400)
        {
            GetOpponentToPlayer(activePlayer).gold -= 400;
            uiManager.ChangePlayerUI(GetOpponentToPlayer(activePlayer), GetOpponentToPlayer(activePlayer).team);
            return true;
        }
        return false;
    }
}
