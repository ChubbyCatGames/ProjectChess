using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertsBlessing : Consumable
{
    DesertsBlessing()
    {
        cost = 250;
    }
    public override void OnUse(Piece p)
    {
        p.ChangeBranch();
    }


}
