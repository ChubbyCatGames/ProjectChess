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

    private void Awake()
    {
        objectName = "Alchemist secret";
        objectDescription = "(Consumable) Heals all the life of a unit and removes every negative effects such as poison and immobilization";
        cost =400;
    }

}
