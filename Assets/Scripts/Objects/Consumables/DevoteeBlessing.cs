using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevoteeBlessing : Consumable
{
    DevoteeBlessing()
    {
        cost = 400;
    }
    public override void OnUse(Piece p)
    {
        p.blessingDevelopCost -= 1;
    }
}
