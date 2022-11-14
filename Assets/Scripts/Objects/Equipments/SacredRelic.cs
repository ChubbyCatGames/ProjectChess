using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredRelic : Equipment
{
    SacredRelic()
    {
        cost = 700;
    }

    public override void OnUse(Piece p)
    {
        p.ignoreFirstAttack= true;
    }
    public override void OnUnequip(Piece p)
    {
        p.ignoreFirstAttack= false;
    }
}
