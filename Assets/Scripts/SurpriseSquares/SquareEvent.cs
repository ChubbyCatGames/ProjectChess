using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SquareEvent : MonoBehaviour
{
    public string squareName;
    public string squareDescription;
    public abstract void StartEvent(Piece p);

    public void Delete()
    {
        transform.position = new Vector3(100,100,100);
    }
}
