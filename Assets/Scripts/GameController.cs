using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]
public class GameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;

    private PieceCreator pieceCreator;

    //Instances of the players and the active player
    private Player whitePlayer;
    private Player blackPlayer;
    private Player activePlayer;


    private void Awake()
    {
        SetDependencies();
        CreatePLayers();
    }

    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
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
        board.SetDependencies(this);
        CreatePiecesFromLayout(startingBoardLayout);
        activePlayer = whitePlayer;
        GenerateAllPossiblePlayerMoves(activePlayer);
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
    private void CreatePieceAndInitialize(Vector2Int squareCoords, PieceColor pieceColor, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, pieceColor, board);

        Material colorMaterial = pieceCreator.GetPieceMaterial(pieceColor);
        newPiece.SetMaterial(colorMaterial);

        board.SetPieceOnBoard(squareCoords, newPiece);

        Player currentPlayer = pieceColor == PieceColor.White ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(newPiece);
    }

    private void GenerateAllPossiblePlayerMoves(Player player)
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
        ChangeActiveTeam();
        Debug.Log("cambio");

    }
    private Player GetOpponentToPlayer(Player player)
    {
        return player == whitePlayer ? blackPlayer : whitePlayer; 
    }

    private void ChangeActiveTeam()
    {
        activePlayer = activePlayer == whitePlayer ? blackPlayer : whitePlayer; 

    }

}
