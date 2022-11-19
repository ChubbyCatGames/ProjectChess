using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RickTheMerchant : SquareEvent
{

    [SerializeField] private List<GameObject> listObjects = new List<GameObject>();

    private List<GameObject> objectsOnSale = new List<GameObject>();
    
    public override void StartEvent(Piece p)
    {
        //INSERTAR LA LLAMADA A LA TIENDA
        //Select 3 objetos random
        SelectThreeObj();
        p.board.OpenShop(objectsOnSale);

    }

    private void Awake()
    {
        squareName = "Rick, the merchant";
        squareDescription = "Rick is here to sell you the best equipment and every type of odd trinkets. Make a good use of his offers, it's an unique opportunity";
    }

    private void SelectThreeObj()
    {
        for (int i = 0; i < 3; i++)
        {
        objectsOnSale.Add(listObjects[Random.Range(0,listObjects.Count-1)]);
        }
        
    }
}

