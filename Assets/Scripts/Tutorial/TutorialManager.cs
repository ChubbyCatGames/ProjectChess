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
            //popUp[0]  Mensaje de bienvenida y dice al jugador que mueva una pieza
            //popUp[1]  Bien hecho y aventurate a probar mas casillas sorpresas, quizás tengas suerte
            //popUp[2]  Explicacion de porque han subido los recursos (economia) y subir pieza a caballo o alfil
            //popUp[3]  Foto de las posibles ascensiones
            //popUp[4]  Mensaje de entabla pelea
            //popUp[5]  Decirte buena pelea y explicar las peleas con foto
            //popUp[6]  Ganar el combate y aparezca la exlicacion de que hay que matar al rey
            //Tras el popUp 5 comerse al rey
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

       

        if (board.GetPieceOnSquare(new Vector2Int(4,2)) != null || board.GetPieceOnSquare(new Vector2Int(5, 2)) != null || board.GetPieceOnSquare(new Vector2Int(4, 1)) != null || board.GetPieceOnSquare(new Vector2Int(5, 1)) != null || board.GetPieceOnSquare(new Vector2Int(5, 3)) != null || board.GetPieceOnSquare(new Vector2Int(4, 3)) != null)
        {
          popUpIdx ++;  //HAY QUE LIMITAR QUE EL JUGADOR SOLO PUEDA MOVEZ LA PIEZA DE LA SEGUNDA COLUMNA
        }

        else if (popUpIdx == 1)
        {
            if (/*si esta ready pa promote*/)
            {
                popUpIdx++; 
            }

        }
   

        else if (popUpIdx == 2)
        {
            if (/*ya ha ascendido*/)
            {
                popUpIdx++;
            }

        }

        else if (popUpIdx == 3)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIdx++;
            }

        }

        //POPUP de vamos pelea ya ome ya

        else if (popUpIdx == 4)
        {
            if (piece.life < piece.maxLife)
            {
                popUpIdx++;
            }

        }

        

        //pop up de ole buena pelea er tio ahi

        else if (popUpIdx == 5)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIdx++;
            }

        }

        //fotito de  objetivo de la aprtida

        else if (popUpIdx == 6)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIdx++;
            }

        }

        //Despues de esta pantalla generar pieza y que se coma al rey


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
