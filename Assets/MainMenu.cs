using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun;

public class MainMenu : MonoBehaviourPunCallbacks
{

public InputField InputFieldNameRoom;

public void CreateRoom()
{
RoomOptions roomOptions = new RoomOptions();
roomOptions.MaxPlayers = 4;
PhotonNetwork.CreateRoom(InputFieldNameRoom.text,
roomOptions);
}
public void JoinRoom()
{
PhotonNetwork.JoinRoom(InputFieldNameRoom.text);
}
public override void OnJoinedRoom()
{
PhotonNetwork.LoadLevel("Game");
}
}
