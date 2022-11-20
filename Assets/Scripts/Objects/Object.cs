using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public int cost;
    public string objectName;
    public string objectDescription;
    public abstract void OnUse(Piece p);
    public virtual void OnUnequip(Piece p)
    {
        return;
    }

}
