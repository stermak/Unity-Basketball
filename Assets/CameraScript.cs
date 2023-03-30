using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject ball;
    private List<GameObject> players;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
    }

    void Update()
    {
        GameObject ballHolder = null;

        foreach (GameObject player in players)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsBallInHands)
            {
                ballHolder = player;
                break;
            }
        }

        if (ballHolder != null)
        {
            transform.LookAt(ballHolder.transform);
        }
        else
        {
            transform.LookAt(ball.transform);
        }
    }
}
