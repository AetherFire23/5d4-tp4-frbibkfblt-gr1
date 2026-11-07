using System;
using System.Collections.Generic;
using Unity.Collections;
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
    private bool _stationaryFlightActive = false;
    private bool _wingsActive = false;
    private HashSet<Vector3> _staticFlightDirections = new HashSet<Vector3>();

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
        print(Mathf.Lerp(mouseX, 0, Vector3.Dot(_rb.linearVelocity, transform.forward)) * 0.02f);
        xRotation -= Mathf.Lerp(mouseX, 0, Vector3.Dot(_rb.linearVelocity, transform.forward) * 0.02f);
        yRotation -= mouseY;

        // 3. Apply vertical rotation directly to this camera
        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);

        HandleInput();
    }

    private void HandleInput()
    {
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _stationaryFlightActive = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _stationaryFlightActive = false;
            _rb.constraints = RigidbodyConstraints.None;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _staticFlightDirections.Add(transform.forward);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _staticFlightDirections.Add(-transform.right);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _staticFlightDirections.Add(-transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _staticFlightDirections.Add(transform.right);
        }

        if (_wingsActive)
        {
            _staticFlightDirections.Add(transform.up);
        }
    }

    void FixedUpdate()
    {
        // print(_isOnGround ? "The bird is on ground" : "The bird is not on ground");
        if (_wingsActive && !_stationaryFlightActive)
        {
            _rb.linearDamping = 0.05f;
            _rb.AddRelativeForce(Vector3.forward * (forwardBias * wingForce * Time.deltaTime), ForceMode.Force);
            _rb.AddForce(Vector3.up * (upBias * wingForce * Time.deltaTime), ForceMode.Force);
        }
        else if (_stationaryFlightActive)
        {
            foreach (var staticFlightDirection in _staticFlightDirections)
            {
                _rb.AddForce(staticFlightDirection * Time.deltaTime * wingForce /2, ForceMode.Force);
            }

            _rb.linearDamping = 0.95f;
        }

        _staticFlightDirections.Clear();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            _rb.linearDamping = 0.95f;
            _rb.angularDamping = 0.95f;
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