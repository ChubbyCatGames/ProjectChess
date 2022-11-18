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

    private void Awake()
    {
        objectName = "Sacred Relic";
        objectDescription = "(Equipment) Grants a shield to the piece that cancels the damage of the first attack received in a duel";
        cost =700;
    }
}
