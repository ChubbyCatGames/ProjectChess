using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BurnishArmor : Equipment
{
    BurnishArmor()
    {
        cost = 300;
        lifeAddition = 15;
    }


    public override void OnUse(Piece p)
    {
        p.life += lifeAddition;
    }
    public override void OnUnequip(Piece p)
    {
        p.life -= lifeAddition;
    }

    private void Awake()
    {
        objectName = "Burnish armor";
        objectDescription = "(Equipment) Grants 15 maximum health to the unit";
        cost = 300;
    }
}
