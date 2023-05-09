using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneSwitcher : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (SceneManager.GetActiveScene().name == "SnowScene")
            {
                SceneManager.LoadScene("VaporScene");
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (SceneManager.GetActiveScene().name == "VaporScene")
            {
                SceneManager.LoadScene("SnowScene");
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}