using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PieceCreator))]
public class GameController : MonoBehaviour
{
    private enum GameState { Init, Play, Finished, Fight}

    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    public TextMeshProUGUI uiText;

    private PieceCreator pieceCreator;
    private CameraManager cameraManager;

    //Instances of the players and the active player
    private Player whitePlayer;
    private Player blackPlayer;
    public Player activePlayer;

    private GameState gameState;


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
        activePlayer = whitePlayer;
        GenerateAllPossiblePlayerMoves(activePlayer);
        SetGameState(GameState.Play);
    }

    private void SetGameState(GameState state)
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



    //Gets called from above and initializes every piece
    public void CreatePieceAndInitialize(Vector2Int squareCoords, PieceColor pieceColor, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, pieceColor, board);

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
        GenerateAllPossiblePlayerMoves(activePlayer);
        GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(activePlayer));
        if (CheckIfGameIsFinished())
            EndGame();
        else
            ChangeActiveTeam();
        

    }

    private bool CheckIfGameIsFinished()
    {
        Piece[] kingAttackingPieces = activePlayer.GetPieceAttakingPieceOfType<King>();
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

    public Piece startFight(Piece attacker, Piece defensor)
    {
        while(attacker.life>0 && defensor.life > 0)
        {
            attacker.Attack(defensor);
            if (defensor.life > 0)
                defensor.Attack(attacker);
        }
        if (defensor.life <= 0)
            return attacker;
        else
            return defensor;
    }

    private void EndGame()
    {
        uiText.text = activePlayer.team.ToString() + " wins";
        SetGameState(GameState.Finished); 
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
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer;
        cameraManager.ChangeCam();

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
}
