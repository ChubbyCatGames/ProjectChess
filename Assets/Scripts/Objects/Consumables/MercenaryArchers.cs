using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryArchers : Consumable
{
    MercenaryArchers()
    {
        cost = 450;
    }
    public override void OnUse(Piece p)
    {
        p.life -= 40f;
    }

    private void Awake()
    {
        objectName = "Mercenary archers";
        objectDescription = "(Consumable) Deals 40 damage to a selected enemy unit. (Take care, the King cannot be the target of this item)";
        cost =450;
    }

}
