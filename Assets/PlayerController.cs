using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public BallController ballController;
    public int playerNumber = 1;
    public float MoveSpeed = 100;
    public float RotationSpeed = 720;
    public float minX = -0f;
    public float maxX = 0f;
    public float minZ = -0f;
    public float maxZ = 0f;
    public bool IsBallInHands { get; private set; } = false;
    public GameObject BallPrefab;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    private PhotonView photonView;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject ballParent;
    [HideInInspector] public GameObject tempBall;

    private bool IsBallFlying = false;
    private float T = 0;
    private float holdTimer = 0f;

    Vector3 ClampPosition(Vector3 position)
    {
        float x = Mathf.Clamp(position.x, minX, maxX);
        float z = Mathf.Clamp(position.z, minZ, maxZ);
        return new Vector3(x, position.y, z);
    }

void Start()
{
    rb = GetComponent<Rigidbody>();
    photonView = GetComponent<PhotonView>();
    if (photonView.IsMine)
    {
        ballParent = new GameObject("BallParent");
        ballParent.transform.SetParent(transform);

        // Assign tempBall to the BallPrefab when created
        GameObject newBall = Instantiate(BallPrefab);
        tempBall = newBall;
        tempBall.transform.SetParent(ballParent.transform);
        tempBall.transform.localPosition = Vector3.zero;
        tempBall.GetComponent<Rigidbody>().isKinematic = true;

        ballController = tempBall.GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.ballAudioSource.enabled = false;
        }
    }
}


    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;
        if (!photonView.IsMine) return;

if (!IsBallInHands && !IsBallFlying)
{
    if (ballController != null)
    {
        ballController.ballAudioSource.enabled = true;
    }
}


        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.D))
            {
                horizontalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                horizontalInput = 1f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                verticalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                verticalInput = 1f;
            }
        }
        else if (playerNumber == 2)
        {
            if (Input.GetKey(KeyCode.L))
            {
                horizontalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.J))
            {
                horizontalInput = 1f;
            }

            if (Input.GetKey(KeyCode.I))
            {
                verticalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                verticalInput = 1f;
            }
        }

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.fixedDeltaTime);
        }
        rb.MovePosition(ClampPosition(transform.position + direction * MoveSpeed * Time.fixedDeltaTime));

        void LookAtTarget()
        {
            Vector3 directionToTarget = (Target.position - transform.position).normalized;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);
        }

        if (IsBallInHands)
        {
            if (Input.GetKey(KeyCode.Space) && tempBall.transform.parent == transform)
            {
                tempBall.transform.position = PosOverHead.position;
                                Arms.localEulerAngles = Vector3.right * 180;
                holdTimer += Time.deltaTime;
            }
            else
            {
                tempBall.transform.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }

if (Input.GetKeyUp(KeyCode.Space))
{
    LookAtTarget();
    IsBallInHands = false;
    IsBallFlying = true;
    tempBall.transform.parent = null;
    tempBall.GetComponent<Rigidbody>().isKinematic = false;
    T = 0;

    float throwForce = 10f;
    Vector3 throwDirection = (Target.position - tempBall.transform.position).normalized;
    tempBall.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);
}

        }

        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B;
            if (holdTimer >= 1f && holdTimer <= 2f)
            {
                B = Target.position;
            }
            else if (holdTimer >= 0f && holdTimer <= 1f || holdTimer >= 2f && holdTimer <= 3f)
            {
                B = A + transform.forward * 3f + Vector3.up * 1f;
            }
            else
            {
                B = A + transform.forward * 3f;
            }
            Vector3 pos = Vector3.Lerp(A, B, t01);

            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            tempBall.transform.position = pos + arc;

            if (t01 >= 1)
            {
                IsBallFlying = false;
                tempBall.GetComponent<Rigidbody>().isKinematic = false;
                ballController.ballAudioSource.enabled = true;
                holdTimer = 0f;

                float distanceToBall = Vector3.Distance(transform.position, tempBall.transform.position);
                if (distanceToBall < 2f)
                {
                    IsBallInHands = false;
                }
            }
        }
    }

private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("Player") && other.gameObject != gameObject)
    {
        if (photonView.IsMine)
        {
            tempBall.transform.SetParent(ballParent.transform);
            PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
            if (otherPlayer.IsBallInHands)
            {
                otherPlayer.IsBallInHands = false;
                IsBallInHands = true;

                ballParent.transform.position = transform.position;
                ballParent.transform.rotation = transform.rotation;

                tempBall.transform.SetParent(ballParent.transform);
                tempBall.transform.localPosition = Vector3.zero;
                tempBall.GetComponent<Rigidbody>().isKinematic = true;

                ballController = tempBall.GetComponent<BallController>();
                if (ballController != null)
                {
                    ballController.ballAudioSource.enabled = false;
                }
            }
        }
    }
    else if (other.gameObject.CompareTag("Ball") && !IsBallFlying)
    {
        tempBall = other.gameObject;
        tempBall.transform.SetParent(ballParent.transform);
        IsBallInHands = true;

        ballParent.transform.position = transform.position;
        ballParent.transform.rotation = transform.rotation;

        tempBall.transform.SetParent(ballParent.transform);
        tempBall.transform.localPosition = Vector3.zero;
        tempBall.GetComponent<Rigidbody>().isKinematic = true;

        ballController = tempBall.GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.ballAudioSource.enabled = false;
        }
    }
}


}