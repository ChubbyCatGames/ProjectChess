using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistSecret : Consumable
{
    AlchemistSecret()
    {
        cost = 400;
    }
    public override void OnUse(Piece p)
    {
        p.life = p.maxLife;
    }

}
