using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.life /= 2;

        p.board.particleManager.PlayArrowParticles(p.board.CalculatePositionFromCoords(p.occupiedSquare));

        GameObject.Find("AudioManager").GetComponent<AudioManager>().archers.Play();
    }

    private void Awake()
    {
        squareName = "Unespected ambush";
        squareDescription = "The unit loses half of its health";
    }
}
