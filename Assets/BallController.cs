using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviour
{
    public AudioSource ballAudioSource;
    public AudioClip bounceSound;
    private float RandomNumber = 7;


    void Start()
    {
        ballAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 0.5f)
        {
            ballAudioSource.PlayOneShot(bounceSound, 1f);
        }
    }

}