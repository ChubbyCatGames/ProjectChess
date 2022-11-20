using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearsTrap : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        p.canMoveNextTurn = false;

        p.board.particleManager.PlayLockParticles(p.board.CalculatePositionFromCoords(p.occupiedSquare));

        GameObject.Find("AudioManager").GetComponent<AudioManager>().lockEvent.Play();
    }

    private void Awake()
    {
        squareName = "Bear Trap";
        squareDescription = "The unit is locked and can't move during the next turn";
    }

}
