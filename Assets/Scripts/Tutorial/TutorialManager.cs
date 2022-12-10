using ExitGames.Client.Photon.StructWrapping;
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
    private List<Piece> pieces; 
    public GameObject[] popUps; 

    // Start is called before the first frame update
    void Start()
    {
        popUpIdx = 0;
        iaTurn = false;
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        pieces = board.controller.activePlayer.activePieces;
        
            for (int i = 0; i < popUps.Length; i++)
        {
            //popUp[0]  Mensaje de bienvenida
            //popUP[1]  dice al jugador que mueva una pieza
            //popUp[2]  Bien hecho y aventurate a probar mas casillas sorpresas, quizás tengas suerte
            //popUp[3]  Explicacion de porque han subido los recursos (economia) y subir pieza a caballo o alfil
            //popUp[4]  Foto de las posibles ascensiones
            //popUp[5]  Mensaje de entabla pelea
            //popUp[6]  Decirte buena pelea y explicar las peleas con foto
            //popUp[7]  Ganar el combate y aparezca la exlicacion de que hay que matar al rey
            //Tras el popUp 5 comerse al rey
            

            if (i == popUpIdx)
            {
                
                popUps[i].SetActive(true);
                if (popUpIdx > 0)
                {
                    popUps[i - 1].SetActive(false);
                }
            }
           
        }
        //Debug.Log(board.getSelectedPiece().occupiedSquare);
            //mueva una pieza

       if (popUpIdx == 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIdx++;
            }

        }

        else if ((board.GetPieceOnSquare(new Vector2Int(4,2)) != null || board.GetPieceOnSquare(new Vector2Int(5, 2)) != null || board.GetPieceOnSquare(new Vector2Int(4, 1)) != null || board.GetPieceOnSquare(new Vector2Int(5, 1)) != null || board.GetPieceOnSquare(new Vector2Int(5, 3)) != null || board.GetPieceOnSquare(new Vector2Int(4, 3)) != null) && popUpIdx==1)
        {
          popUpIdx ++;  //HAY QUE LIMITAR QUE EL JUGADOR SOLO PUEDA MOVEZ LA PIEZA DE LA SEGUNDA COLUMNA
        }
        
        else if (popUpIdx == 2)
        {
            while (board.controller.activePlayer.gold < 80 && board.controller.activePlayer.blessing < 3)
            {
                board.controller.activePlayer.UpdateGold();
                board.controller.activePlayer.blessing += 1;
            }
            
            
            if (/*si esta ready pa promote*/ board.controller.activePlayer.gold >= 75 && board.controller.activePlayer.blessing >=2)
            {
                popUpIdx++; 
            }

        }
   

        else if (popUpIdx == 3)
        {
            //Si ha promote
            foreach(var p in pieces)
            {
                if (p.IsType<Bishop>() || p.IsType<Knight>())
                {
                    popUpIdx++;
                }
            }

        }

        else if (popUpIdx == 4)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIdx++;
            }

        }

        //POPUP de vamos pelea ya ome ya

        else if (popUpIdx == 5)
        {
            if (piece.life < piece.maxLife)
            {
                popUpIdx++;
            }

        }

        

        //pop up de ole buena pelea er tio ahi

        else if (popUpIdx == 6)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIdx++;
            }

        }

        //fotito de  objetivo de la aprtida

        else if (popUpIdx == 7)
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
        pieces = board.controller.activePlayer.activePieces;

    }
}
