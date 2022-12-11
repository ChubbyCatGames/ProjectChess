using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int popUpIdx;

    [SerializeField] Board board;
    private Piece piece;
    private List<Piece> pieces; 
    public GameObject[] popUps;
    float waitTime = 2f;
    bool pasaTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        popUpIdx = 0;
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime <= 0 && pasaTurn)
        {
            board.EndTurn();
            pasaTurn = false;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }

        pieces = board.controller.activePlayer.activePieces;
        
            for (int i = 0; i < popUps.Length; i++)
        {
            //popUp[0]  Mensaje de bienvenida
            //popUP[1]  dice al jugador que mueva una pieza
            //popUp[2]  Bien hecho, ahora pasa turno y deja que mueva el contrario
            //popUp[3]  TUS recursos suben con el principio de un nuevo turno!!
            //popUp[4]  Explicacion de porque han subido los recursos (economia)
            //popUp[5]  asciende la pieza o que
            //popUp[6]  subir pieza a caballo o alfil
            //popUp[7]  Foto de las posibles ascensiones
            //popUp[8]  Mensaje de entabla pelea
            //popUp[9]  Decirte buena pelea y explicar las peleas con foto
            //popUp[10]  Ganar el combate y aparezca la exlicacion de que hay que matar al rey
            //Tras el popUp 10 comerse al rey
            

            //HACE FALTA INHABILITAR EL MOVIMIENTO DEL REY


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

        else if ((board.GetPieceOnSquare(new Vector2Int(4,3)) != null || board.GetPieceOnSquare(new Vector2Int(4, 2)) != null) && popUpIdx==1)
        {

            Debug.Log(board.GetPieceOnSquare(new Vector2Int(4, 1)));
            popUpIdx++; //HAY QUE LIMITAR QUE EL JUGADOR SOLO PUEDA MOVEZ LA PIEZA DE LA SEGUNDA COLUMNA
                
            
            
        }


        else if (popUpIdx == 3)
        {
            

            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
            }

        }

        else if (popUpIdx == 4)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
            }

        }


        else if (popUpIdx == 5)
        {
            while (board.controller.activePlayer.gold < 80 && board.controller.activePlayer.blessing < 3)
            {
                board.controller.activePlayer.UpdateGold();
                board.controller.activePlayer.blessing += 1;
                board.uIManager.ChangePlayerUI(board.controller.activePlayer,board.controller.activePlayer.team);
            }
            
            
            if (/*si esta ready pa promote*/ board.controller.activePlayer.gold >= 75 && board.controller.activePlayer.blessing >=2)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    popUpIdx++;
                }
            }

        }
   

        else if (popUpIdx == 6)
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

        else if (popUpIdx == 7)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
            }

        }

        //POPUP de vamos pelea ya ome ya

        else if (popUpIdx == 8)
        {
            foreach (var p in pieces)
                if (p.life < p.maxLife)
            {
                popUpIdx++;
            }

        }

        

        //pop up de ole buena pelea er tio ahi

        else if (popUpIdx == 9)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
            }

        }

        else if (popUpIdx == 10)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
            }

        }

        //fotito de  objetivo de la aprtida

        else if (popUpIdx == 11)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
                
            }

        }

        //Despues de esta pantalla generar pieza y que se coma al rey
        else if(popUpIdx > 11)
        {
            if (waitTime <= 0 && pasaTurn)
            {
                board.EndTurn();
                pasaTurn = false;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                popUpIdx++;
            }
        }

    }


    public void NextTurn()
    {

        if (popUpIdx == 2)
        {
            
            popUpIdx++;
            Vector2Int pos = new Vector2Int(4, 6);
            piece = board.GetPieceOnSquare(new Vector2Int(4, 6));
            board.MoveAuto(piece, pos - new Vector2Int(0, 1));
            pieces = board.controller.activePlayer.activePieces;
            pasaTurn = true;
            waitTime = 2f;

        }

        if(popUpIdx > 11)
        {
            board.EndTurn();
        }
        pasaTurn = true;
        waitTime = 2f;


    }
}
