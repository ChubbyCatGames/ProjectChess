using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class JoinRoom : MonoBehaviourPunCallbacks
{
    public int roomId;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void JoinARoom()
    {
        //Debug.Log(inputField.text.ToString());
        //PhotonNetwork.JoinRoom(inputField.ToString());

        PhotonNetwork.JoinRandomRoom();

        Debug.Log("me uni");
        text.text = inputField.text;
       
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateNewRoom();
    }

    private void CreateNewRoom()
    {
        roomId = Random.Range(0, 100);

        PhotonNetwork.JoinOrCreateRoom("Sala no." + roomId, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);

        PhotonNetwork.LoadLevel("FirstTry");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        CreateNewRoom();
    }
}
