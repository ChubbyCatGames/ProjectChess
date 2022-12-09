using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int popUpIdx;
    private bool iaTurn;
    [SerializeField] Board board;
    private Piece piece;

    public GameObject[] popUps; 

    // Start is called before the first frame update
    void Start()
    {
        popUpIdx = 0;
        iaTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i== popUpIdx)
            {
               // popUps[i].SetActive(true);
            }
            else
            {
               // popUps[i].SetActive(false);
            }
        }
        //Debug.Log(board.getSelectedPiece().occupiedSquare);
            //mueva una pieza
            if (board.GetPieceOnSquare(new Vector2Int(4,2)) != null){
                Debug.Log("hola");
            }
        
        
    }


    public void NextTurn()
    {
        iaTurn = true;
        //popUpIdx += 1;
        Vector2Int pos = new Vector2Int(4, 6);
        piece = board.GetPieceOnSquare(new Vector2Int(4, 6));
        board.MoveAuto(piece, pos - new Vector2Int(0, 1));

    }
}
