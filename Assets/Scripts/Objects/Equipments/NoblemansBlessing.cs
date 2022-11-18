using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class NoblemansBlessing : Equipment
{
    NoblemansBlessing()
    {
        cost = 200;
        richnessAddition = 2;
    }
    public override void OnUse(Piece p)
    {
        p.richness *= richnessAddition;
    }
    public override void OnUnequip(Piece p)
    {
        p.richness/= richnessAddition; 
    }

    private void Awake()
    {
        objectName = "Nobleman's Blessing";
        objectDescription = "(Equipment) The unit gives double tithe";
        cost =200;
    }
}
