using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryArchers : Object
{
    MercenaryArchers()
    {
        objectName = "Mercenary archers";
        objectDescription = "(Consumable) Deals 40 damage to a selected enemy unit. (Take care, the King cannot be the target of this item)";
        cost = 450;
    }
    public override void OnUse(Piece p)
    {
        p.life -= 40f;

        p.board.particleManager.PlayArrowParticles(p.board.CalculatePositionFromCoords(p.occupiedSquare));

        GameObject.Find("AudioManager").GetComponent<AudioManager>().archers.Play();
    }

}
