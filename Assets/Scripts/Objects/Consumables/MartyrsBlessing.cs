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

    private void Awake()
    {
        objectName = "Martyr's Blessing";
        objectDescription = "(Consumable) The unit doubles its attack damage until the turn comes back the next time to the player. Then, the unit dies";
        cost =500;
    }
}
