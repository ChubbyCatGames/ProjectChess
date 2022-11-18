using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBanner : Equipment
{

    WarBanner()
    {
        cost = 500;
    }
    public override void OnUse(Piece p)
    {
        p.duplicatePassive = 2;
    }
    public override void OnUnequip(Piece p)
    {
        p.duplicatePassive = 1;
    }

    private void Awake()
    {
        objectName = "War banner";
        objectDescription = "(Equipment) The passive ability of the unit has double effectivity";
        cost = 500;
    }
}