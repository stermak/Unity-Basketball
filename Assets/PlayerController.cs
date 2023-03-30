using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    private Rigidbody rb;
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
    }

    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

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
            directionToTarget.y = 0; // ”бираем вертикальную составл€ющую, чтобы игрок не "смотрел" вверх или вниз
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);
        }

        if (IsBallInHands)
        {
            if (Input.GetKey(KeyCode.Space) && Ball.parent == transform)
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;
                holdTimer += Time.deltaTime;
            }
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                LookAtTarget();
                IsBallInHands = false;
                IsBallFlying = true;
                Ball.parent = null;
                T = 0;
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

            Ball.position = pos + arc;

            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                ballController.ballAudioSource.enabled = true;
                holdTimer = 0f;

                float distanceToBall = Vector3.Distance(transform.position, Ball.position);
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
            PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
            if (otherPlayer.IsBallInHands)
            {
                otherPlayer.IsBallInHands = false;
                IsBallInHands = true;
                Ball.parent = transform;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                ballController.ballAudioSource.enabled = true;
            }
        }
        else if (other.gameObject.CompareTag("Ball") && !IsBallFlying)
        {
            IsBallInHands = true;
            Ball.parent = transform;
            Ball.GetComponent<Rigidbody>().isKinematic = false;
            ballController.ballAudioSource.enabled = true;
        }
    }
}