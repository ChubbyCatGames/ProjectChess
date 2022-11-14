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

}
