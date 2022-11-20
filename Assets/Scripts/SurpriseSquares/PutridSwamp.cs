using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutridSwamp : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.poisoned= true;

        p.board.particleManager.PlayPoisonParticles(p.board.CalculatePositionFromCoords(p.occupiedSquare));

        GameObject.Find("AudioManager").GetComponent<AudioManager>().poisonEvent.Play();
    }


    private void Awake()
    {
        squareName = "Putrid swamp";
        squareDescription = "The unit gets poisoned and loses 5 health points each turn";
    }
}
