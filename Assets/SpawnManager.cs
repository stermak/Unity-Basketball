using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    public Transform[] Spawns;

    public GameObject Player;
    public GameObject Player2;
    public GameObject Ball;
    public GameObject Hoop;
    public GameObject Hoop2;
    public GameObject cameraPrefab;

    void Awake()
    {
        Vector3 cameraPosition = new Vector3(0f, 400f, 200f);
        Quaternion cameraRotation = Quaternion.Euler(65f, 180f, 0f);

        PhotonNetwork.Instantiate(cameraPrefab.name, cameraPosition, cameraRotation);
        PhotonNetwork.Instantiate(Player2.name, Spawns[0].position, Quaternion.identity);
        PhotonNetwork.Instantiate(Player.name, Spawns[1].position, Quaternion.identity);
        PhotonNetwork.Instantiate(Ball.name, Spawns[4].position, Quaternion.identity);
        PhotonNetwork.Instantiate(Hoop.name, Spawns[2].position, Quaternion.identity);
        PhotonNetwork.Instantiate(Hoop2.name, Spawns[3].position, Quaternion.identity);
    }
}