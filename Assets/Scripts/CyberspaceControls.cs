using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CyberspaceControls : MonoBehaviour
{

    public float forceMultiplier;

    public float reloadTime = 1.0f;
    private float reloadTimer = 0.0f;

    public Transform bullet;

    private PlayerInput input;

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Horizontal view rotation
        GetComponent<Rigidbody>().MoveRotation(
            GetComponent<Rigidbody>().rotation
            * Quaternion.AngleAxis(input.actions["Look"].ReadValue<Vector2>().x, Vector3.up)
            );
        // Vertical view rotation
        GetComponent<Rigidbody>().MoveRotation(
            GetComponent<Rigidbody>().rotation
            * Quaternion.AngleAxis(-input.actions["Look"].ReadValue<Vector2>().y, Vector3.right)
        );

        // Rolling
        GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * Quaternion.AngleAxis(- input.actions["Roll"].ReadValue<float>() * Time.deltaTime * 50, Vector3.forward));

        // Shooting
        reloadTimer += Time.deltaTime;
        if (reloadTimer > reloadTime && input.actions["Shoot"].ReadValue<float>() > 0)
        {
            Quaternion rotation = transform.rotation * Quaternion.AngleAxis(Random.Range(-2.0f, 2.0f), Vector3.up)
            * Quaternion.AngleAxis(Random.Range(-2.0f, 2.0f), Vector3.right);

            Instantiate(bullet, transform.position, rotation);
            GetComponent<Rigidbody>().AddForce(-3.0f * transform.forward * 0.02f, ForceMode.Impulse);

            reloadTimer = 0;
        }
    }

    void FixedUpdate()
    {
        // Movement
        GetComponent<Rigidbody>().AddRelativeForce(forceMultiplier * input.actions["Movement"].ReadValue<Vector2>().x * Vector3.right);
        GetComponent<Rigidbody>().AddRelativeForce(forceMultiplier * input.actions["Movement"].ReadValue<Vector2>().y * Vector3.forward);
    }
}
