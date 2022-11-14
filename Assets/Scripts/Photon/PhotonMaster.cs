using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;

public class PhotonMaster : MonoBehaviourPunCallbacks
{

    PunTurnManager punTurn;
    int playerCount = 0;

    [SerializeField]Canvas c;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Se va a conectar");


        punTurn = new PunTurnManager();

        
    }

    private void Update()
    {
        Debug.Log("Tamos conectados");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Se ha conectado");
        //PhotonNetwork.CreateRoom("sala");

        PhotonNetwork.AutomaticallySyncScene = true;

        
        
    }

    public override void OnCreatedRoom()
    {
        
        Debug.Log("eyu");

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("me uni");
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerCount++;
            Debug.Log(playerCount);

            // hide and close the room if it is full
            if (playerCount == 1)
            {
                SendStartGame();
            }
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("lobby");
    }

    private void SendStartGame()
    {
        SendMessage("StartGame");
    }

    public void StartGame()
    {
        c.enabled = false;
    }

    public void JoinRoom()
    {
        //PhotonNetwork.JoinOrCreateRoom("Hola");
    }

    

    
}
