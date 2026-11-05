using UnityEngine;

public class NoclipPlayerController : MonoBehaviour
{
    public float maxForwardSpeed = 100f;
    public float maxUpSpeed = 100f;
    public float wingForce = 10f;
    public float upBias = 0.5f;
    public float forwardBias = 1f;

    private float _speed;
    private bool _isOnGround;
    private bool _wingsActive = false;

    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
    }

    private float xRotation = 0f;

    private float yRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        _rb.maxLinearVelocity = maxForwardSpeed;
        // 1. Get Mouse input multiplied by sensitivity
        float mouseX = Input.GetAxis("Mouse X") * 5;
        float mouseY = Input.GetAxis("Mouse Y") * 5;
        // 2. Calculate vertical camera rotation (Look Up/Down)
        xRotation -= mouseX;
        yRotation -= mouseY;

        // 3. Apply vertical rotation directly to this camera
        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);

        if (Input.GetButtonDown("Jump"))
        {
            _wingsActive = true;
            if (_isOnGround)
                _rb.AddForce(transform.up * wingForce * Time.deltaTime, ForceMode.Impulse);
        }

        if (Input.GetButtonUp("Jump"))
        {
            _wingsActive = false;
        }
    }

    void FixedUpdate()
    {
        // print(_isOnGround ? "The bird is on ground" : "The bird is not on ground");
        if (_wingsActive)
        {
            _rb.AddRelativeForce(Vector3.forward * (forwardBias * wingForce * Time.deltaTime), ForceMode.Force);
            _rb.AddForce(Vector3.up * (upBias * wingForce * Time.deltaTime), ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            _rb.linearDamping = 0.95f;
            _rb.linearVelocity /= 10;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = false;
            _rb.linearDamping = 0.1f;
        }
    }

    bool ForceFlightActive()
    {
        return _wingsActive;
    }
}