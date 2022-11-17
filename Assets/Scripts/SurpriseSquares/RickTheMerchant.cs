using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RickTheMerchant : SquareEvent
{
    public override void StartEvent(Piece p)
    {
        //INSERTAR LA LLAMADA A LA TIENDA
    }

    private void Awake()
    {
        squareName = "Rick, the merchant";
        squareDescription = "Rick is here to sell you the best equipment and every type of odd trinkets. Make a good use of his offers, it's an unique opportunity";
    }
}

