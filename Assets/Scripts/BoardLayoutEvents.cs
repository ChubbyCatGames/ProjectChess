using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/LayoutEvents")]

//This is a scriptable object that appears inside the editor to place connect the positions with every piece
public class BoardLayoutEvents : ScriptableObject
{
    [Serializable]
    private class BoardSetupEvents
    {
        public Vector2Int position;
        public SquareEvent _event;

    }
    private SquareEvent tempEvent;
    [SerializeField] BoardSetupEvents[] boardSetupEvents;

    public int GetPiecesCount()
    {
        return boardSetupEvents.Length;
    }

    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if (boardSetupEvents.Length <= index)
        {
            Debug.LogError("Index of piece out of Range");
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(boardSetupEvents[index].position.x - 1, boardSetupEvents[index].position.y - 1);
    }

    public SquareEvent GetSquareEventNameAtIndex(int index)
    {
        if (boardSetupEvents.Length <= index)
        {
            Debug.LogError("Index of piece out of Range");
            return null;
        }

        return boardSetupEvents[index]._event;
    }

    public void ShuffleEvents()
    {
        for (int i = 0; i < boardSetupEvents.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(0,boardSetupEvents.Length);
            tempEvent = boardSetupEvents[rnd]._event;
            boardSetupEvents[rnd]._event = boardSetupEvents[i]._event;
            boardSetupEvents[i]._event = tempEvent;
        }
    }
}

