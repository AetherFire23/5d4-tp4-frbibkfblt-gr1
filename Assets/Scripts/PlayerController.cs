using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
    private bool _isPerching = false;
    private Vector3 perchPoint = Vector3.zero;

    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _rb.maxLinearVelocity = maxForwardSpeed;
        _rb.maxAngularVelocity = maxForwardSpeed;
    }

    private float xRotation = 0f;

    private float yRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * 5;
        float mouseY = Input.GetAxis("Mouse Y") * 5;
        xRotation -= Mathf.Lerp(mouseX, 0, Math.Abs(_rb.linearVelocity.x) * xCameraLock);
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -100f, 70f);
        _animator.speed = 0.75f;

        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _animator.SetBool("flying", true);
            _animator.SetBool("gliding", false);
            print("Should be flying, not gliding");
            _wingsActive = true;
            if (_isOnGround)
            {
                _animator.SetInteger("hop", 1);
                _rb.AddForce(transform.up * (wingForce * Time.deltaTime * 1f),
                    ForceMode.Impulse); // small jump to kickstart flight
                StartCoroutine(nameof(ResetHopInt), 1);
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (!_isOnGround)
            {
                _animator.SetBool("gliding", true);
            }
            _animator.SetBool("flying", false);
            _wingsActive = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _stationaryFlightActive = true;
            _rb.useGravity = false;
            _rb.linearDamping = 0.95f; // slow down a lot
            _animator.SetBool("flying", true);
            _animator.SetBool("gliding", false);
            _animator.speed = 0.10f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _stationaryFlightActive = false;
            _rb.useGravity = true;
            _rb.linearDamping = 0.00f; // stop slowing down
            _animator.SetBool("flying", false);
            _animator.SetBool("gliding", true);
            _animator.speed = 0.75f;
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

        if (_wingsActive) // go up if flying
        {
            _staticFlightDirections.Add(transform.up);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isOnGround)
        {
            _animator.SetBool("landing", true);
            Perch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isPerching = false;
        }

}

    void ResetHopInt()
    {
        _animator.SetInteger("hop", 0);
    }

    void FixedUpdate()
    {
        if (_isPerching)
        {
            transform.position = Vector3.MoveTowards(transform.position, perchPoint, 10 * Time.deltaTime);
        }

        if (_isOnGround)
        {
            _animator.SetBool("landing", false);
            _isPerching = false;
            _rb.constraints = RigidbodyConstraints.FreezePositionX  | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None;
        }

        
        if (IsFlyingWithWings())
        {
            _rb.AddRelativeForce(Vector3.forward * (forwardBias * wingForce * Time.deltaTime), ForceMode.Force);
            _rb.AddForce(Vector3.up * (upBias * wingForce * Time.deltaTime), ForceMode.Force);
        }
        else if (IsStationaryFlightActive())
        {
            foreach (var staticFlightDirection in _staticFlightDirections)
            {
                _rb.AddForce(staticFlightDirection * (Time.deltaTime * wingForce) / 2, ForceMode.Force);
            }

        }

        if (IsFreeFlying())
        {
            ApplyTorque();
        }

        _previousFacingDirection = transform.forward;
        _staticFlightDirections.Clear();
    }

    private bool IsFlyingWithWings()
    {
        return _wingsActive && !_stationaryFlightActive;
    }

    private bool IsStationaryFlightActive()
    {
        return _stationaryFlightActive;
    }

    private bool IsFreeFlying()
    {
        return !_isOnGround &&  !_stationaryFlightActive;
    }

    /// <summary>
    /// Allows the bird able to turn even when wings are inactive
    /// </summary>
    private void ApplyTorque()
    {
        // slow down bird in direction of velocity
        _rb.AddForce(Vector3.Lerp(Vector3.zero, -_rb.linearVelocity * (torque * Time.deltaTime) / 4, Math.Abs(_rb.linearVelocity.x)),
            ForceMode.Force);
        
        // speed up bird in forward direction
        _rb.AddForce(Vector3.Lerp(Vector3.zero, transform.forward * (torque * 5 * Time.deltaTime), Math.Abs(_rb.linearVelocity.x)),
            ForceMode.Force);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool("gliding", false);
            _animator.SetBool("flying", false);
            // _animator.SetBool("landing", true);
            _isOnGround = true;
            _rb.linearDamping = 0.5f;
            _rb.linearVelocity /= 10;
            _rb.mass = 25;
            _rb.useGravity = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = false;
            _rb.linearDamping = 0.0f;
            _rb.mass = 10;
            _rb.useGravity = true;
        }
    }

    void Perch()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit);
        if (hit.distance < 15f)
        {
            _isPerching = true;
            perchPoint = hit.point;
        }
    }
}