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


    private void Awake()
    {
        SetDependencies();
    }

    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }
    void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        CreatePiecesFromLayout(startingBoardLayout);
    }

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

    private void CreatePieceAndInitialize(Vector2Int squareCoords, PieceColor pieceColor, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, pieceColor, board);

        Material colorMaterial = pieceCreator.GetPieceMaterial(pieceColor);
        newPiece.SetMaterial(colorMaterial);
    }
}
