using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private Transform[] Spawns;

    [SerializeField]private GameObject Player;
    [SerializeField]private GameObject Player2;
    [SerializeField]private GameObject Ball;
    [SerializeField]private GameObject Hoop;
    [SerializeField]private GameObject Hoop2;
    [SerializeField]private GameObject cameraPrefab;

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