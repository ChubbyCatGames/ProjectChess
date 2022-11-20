using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public int roomId;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateNewRoom()
    {
        roomId = Random.Range(0, 100);

        PhotonNetwork.JoinOrCreateRoom( roomId.ToString(), new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);

        text.text = roomId.ToString();
        Debug.Log(roomId);


        PhotonNetwork.LoadLevel("FirstTry");
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        CreateNewRoom();

    }

}
