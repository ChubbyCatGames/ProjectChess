using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class NoblemansBlessing : Object
{
    NoblemansBlessing()
    {
        objectName = "Nobleman's Blessing";
        objectDescription = "(Equipment) The unit gives double tithe";
        cost = 200;

    }
    public override void OnUse(Piece p)
    {
        p.richness *= 2;
    }
    public override void OnUnequip(Piece p)
    {
        p.richness/= 2; 
    }


}
