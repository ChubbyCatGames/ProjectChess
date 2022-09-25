using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]

//This is a scriptable object that appears inside the editor to place connect the positions with every piece
public class BoardLayout : ScriptableObject
{
   [Serializable]
   private class BoardSetup
    {
        public Vector2Int position;
        public PieceType pieceType;
        public PieceColor pieceColor;
    }

    [SerializeField]  BoardSetup[] boardSetup;

    public int GetPiecesCount()
    {
        return boardSetup.Length;
    }

    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if (boardSetup.Length <= index)
        {
            Debug.LogError("Index of piece out of Range");
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(boardSetup[index].position.x-1, boardSetup[index].position.y-1);
    }

    public string GetSquarePieceNameAtIndex(int index)
    {
        if (boardSetup.Length <= index)
        {
            Debug.LogError("Index of piece out of Range");
            return "";
        }

        return boardSetup[index].pieceType.ToString();
    }

    public PieceColor GetSquarePieceColorAtIndex(int index)
    {
        if (boardSetup.Length <= index)
        {
            Debug.LogError("Index of piece out of Range");
            return PieceColor.Black;
        }

        return boardSetup[index].pieceColor;
    }
}
