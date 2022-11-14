using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Object
{
    public float lifeAddition;
    public float dmgAddition;
    public int richnessAddition;

    public abstract void OnUnequip(Piece p);

}
