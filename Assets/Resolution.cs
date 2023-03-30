using UnityEngine;

public class Resolution : MonoBehaviour
{
    public int screenWidth = 1920;
    public int screenHeight = 1080;
    public bool isFullScreen = true;

    private void Start()
    {
        Screen.SetResolution(screenWidth, screenHeight, isFullScreen);
    }
}
