using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretFountain : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.life = p.maxLife;

        p.board.particleManager.PlayHealPieceParticles(p.board.CalculatePositionFromCoords(p.occupiedSquare));

        GameObject.Find("AudioManager").GetComponent<AudioManager>().heal.Play();
    }


    private void Awake()
    {
        squareName = "Hidden fountain";
        squareDescription = "The unit restores all of his health";
    }
}
