using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class RealWorldControls : MonoBehaviour
{
    private bool grounded = false;
    public float speed;

    public float maxDeltaVel = 10.0f;
    public float jumpHeight = 2.0f;

    public bool canJump;

    public Quaternion rotation = Quaternion.identity;

    private float currentLean = 0;

    public float standHeight = 2.3f;
    private float height = 2.3f;
    public float crouchHeight = 1.2f;

    private PlayerInput input;

    // Use this for initialization
    void Start()
    {
        rotation = Quaternion.identity;
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Horizontal view rotation
        rotation *= Quaternion.AngleAxis(input.actions["Look"].ReadValue<Vector2>().x, Vector3.up);

        // Apply leaning
        currentLean = Mathf.Lerp(currentLean, input.actions["Lean"].ReadValue<float>() * 20, Time.deltaTime * 5.0f);
        Quaternion leanRotate = Quaternion.AngleAxis(-currentLean, Vector3.forward);

        GetComponent<Rigidbody>().MoveRotation(rotation * leanRotate);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Freeze rotation and disable gravity
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;

        bool crouching = input.actions["Crouch"].ReadValue<float>() > 0;

        if (grounded)
        {
            Vector2 movement = input.actions["Movement"].ReadValue<Vector2>();
            Vector3 inputDirection = new Vector3(movement.x, 0, movement.y);
            if (inputDirection.magnitude > 1.0)
                inputDirection.Normalize();

            Vector3 targetVel = speed * transform.TransformDirection(inputDirection);

            if (crouching)
            {
                height = crouchHeight;
                targetVel *= 0.7f;
            }
            else
            {
                height = Mathf.Lerp(height, standHeight, Time.fixedDeltaTime * 5.0f);
            }
            this.GetComponent<CapsuleCollider>().height = height;

            Vector3 deltaVel = targetVel - GetComponent<Rigidbody>().velocity;
            deltaVel.x = Mathf.Clamp(deltaVel.x, -maxDeltaVel, maxDeltaVel);
            deltaVel.z = Mathf.Clamp(deltaVel.z, -maxDeltaVel, maxDeltaVel);
            deltaVel.y = 0;
            GetComponent<Rigidbody>().AddForce(deltaVel, ForceMode.VelocityChange);

            if (canJump && input.actions["Jump"].ReadValue<float>() > 0)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x,
                                                 Mathf.Sqrt(2 * jumpHeight * (-Physics.gravity.y)),
                                                 GetComponent<Rigidbody>().velocity.z);
            }
        }

        if (grounded && GetComponent<Rigidbody>().velocity.magnitude > 0.1 && !crouching)
        {
            if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
        }
        else
        {
            if (GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Stop();
        }

        // Manually apply gravity
        GetComponent<Rigidbody>().AddForce(Physics.gravity * GetComponent<Rigidbody>().mass);

        grounded = false;
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint point in collision.contacts)
        {
            float slope = Vector3.Dot(point.normal, Vector3.up);
            if (slope > 0.8)
            {
                grounded = true;
            }
        }

    }
}
