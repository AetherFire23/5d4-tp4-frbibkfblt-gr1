using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NoclipPlayerController : MonoBehaviour
{
    public float maxForwardSpeed = 100f;
    public float wingForce = 10f;
    public float upBias = 0.5f;
    public float forwardBias = 1f;
    public float torque = 10000f;
    public float xCameraLock = 0.2f;

    private float _speed;
    private bool _isOnGround;
    private bool _stationaryFlightActive = false;
    private bool _wingsActive = false;
    private HashSet<Vector3> _staticFlightDirections = new HashSet<Vector3>();
    private Vector3 _previousFacingDirection = Vector3.zero;
    private Animator _animator;

    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private float xRotation = 0f;

    private float yRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        _rb.maxLinearVelocity = maxForwardSpeed;
        _rb.maxAngularVelocity = maxForwardSpeed;
        // 1. Get Mouse input multiplied by sensitivity
        float mouseX = Input.GetAxis("Mouse X") * 5;
        float mouseY = Input.GetAxis("Mouse Y") * 5;
        // 2. Calculate vertical camera rotation (Look Up/Down)
        xRotation -= Mathf.Lerp(mouseX, 0, Math.Abs(_rb.linearVelocity.x) * xCameraLock);
        yRotation -= mouseY;


        // 3. Apply vertical rotation directly to this camera
        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _animator.speed = 1;
            _wingsActive = true;
            if (_isOnGround)
                _rb.AddForce(transform.up * wingForce * Time.deltaTime, ForceMode.Impulse);
        }

        if (Input.GetButtonUp("Jump"))
        {
            _wingsActive = false;
            _animator.speed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _stationaryFlightActive = true;
            _rb.useGravity = false;
            // _rb.constraints = RigidbodyConstraints.Free;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _stationaryFlightActive = false;
            _rb.useGravity = true;
            // _rb.constraints = RigidbodyConstraints.None;
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
        if (_wingsActive && !_stationaryFlightActive)
        {
            _rb.linearDamping = 0.00f;
            _rb.AddRelativeForce(Vector3.forward * (forwardBias * wingForce * Time.deltaTime), ForceMode.Force);
            _rb.AddForce(Vector3.up * (upBias * wingForce * Time.deltaTime), ForceMode.Force);
        }
        else if (_stationaryFlightActive)
        {
            foreach (var staticFlightDirection in _staticFlightDirections)
            {
                _rb.AddForce(staticFlightDirection * Time.deltaTime * wingForce / 2, ForceMode.Force);
            }

            _rb.linearDamping = 0.95f;
        }

        if (!_isOnGround)
        {
            // _rb.AddForce(-_rb.linearVelocity * Time.deltaTime * torque, ForceMode.Force);


            if (!_stationaryFlightActive)
            {
                ApplyTorque();
            }


            // if (!_wingsActive && !_stationaryFlightActive)
            // {
            //     _rb.AddForce(Vector3.up * (upBias * wingForce / 3f * Time.deltaTime), ForceMode.Force);
            // }
        }

        _previousFacingDirection = transform.forward;
        _staticFlightDirections.Clear();
    }

    private void ApplyTorque()
    {
        print(_rb.linearVelocity.x);
        _rb.AddForce(
            Vector3.Lerp(Vector3.zero, -_rb.linearVelocity * torque * Time.deltaTime / 3, Math.Abs(_rb.linearVelocity.x)),
            ForceMode.Force);
        _rb.AddForce(Vector3.Lerp(Vector3.zero, transform.forward * torque /5 * Time.deltaTime, Math.Abs(_rb.linearVelocity.x)),
            ForceMode.Impulse);
        // _rb.AddTorque(_previousRotation * torque, ForceMode.Force);
        // _rb.AddForce(rotationDiff.normalized * torque / 2, ForceMode.Force);
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