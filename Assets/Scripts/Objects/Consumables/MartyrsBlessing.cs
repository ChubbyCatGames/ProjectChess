using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartyrsBlessing : Consumable
{
    public override void OnUse(Piece p)
    {
        p.attackDmg *= 2;
        p.condemned = true;
    }

}
