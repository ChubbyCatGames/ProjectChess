using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistSecret : Object
{
    AlchemistSecret()
    {
        objectName = "Alchemist secret";
        objectDescription = "(Consumable) Heals all the life of a unit and removes every negative effects such as poison and immobilization";
        cost = 400;
    }
    public override void OnUse(Piece p)
    {
        p.life = p.maxLife;


        p.board.particleManager.PlayHealPieceParticles(p.board.CalculatePositionFromCoords(p.occupiedSquare));

        GameObject.Find("AudioManager").GetComponent<AudioManager>().heal.Play();

        p.canMoveNextTurn = true;
        p.poisoned = false;

    }

}
