using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public int cost;
    public abstract void OnUse(Piece p);

}
