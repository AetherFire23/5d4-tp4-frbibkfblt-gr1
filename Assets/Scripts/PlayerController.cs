using System;
using System.Collections.Generic;
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
    private bool _isLanding = false;
    private Vector3 perchPoint = Vector3.zero;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private Rigidbody _rb;

    private void OnLand()
    {
        // animations
        _animator.SetBool("gliding", false);
        _animator.SetBool("flying", false);
        _animator.SetBool("landing", false);
        
        // physics
        _isOnGround = true;
        _isLanding = false;
        _rb.linearDamping = 0.5f;
        _rb.linearVelocity /= 10;
        _rb.mass = 25;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void OnStartFlying()
    {
        _animator.SetBool("flying", true);
        _animator.SetBool("gliding", false);
        print("Should be flying, not gliding");
        _wingsActive = true; // should be in StartFlying
        if (_isOnGround)
        {
            _animator.SetInteger("hop", 1);
            _rb.AddForce(transform.up * (wingForce * Time.deltaTime * 1f),
                ForceMode.Impulse); // small jump to kickstart flight
            StartCoroutine(nameof(ResetHopInt), 1);
        }
    }

    private void OnStartGliding()
    {
    }

    private void OnStopFlying()
    {
        if (!_isOnGround)
        {
            _animator.SetBool("gliding", true);
        }

        _animator.SetBool("flying", false);
        _wingsActive = false;
    }

    private void OnStopGliding()
    {
    }

    private void OnStartLanding()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit);
        if (hit.distance < 15f)
        {
            _animator.SetBool("landing", true);
            _isLanding = true;
            perchPoint = hit.point;
        }
    }

    private void OnStopLanding()
    {
        _isLanding = false;
    }

    private void OnLaunch()
    {
        _isOnGround = false;
        _rb.linearDamping = 0.0f;
        _rb.mass = 10;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
    }

    private void OnFlying()
    {
        _staticFlightDirections.Add(transform.up);
    }

    private void OnGliding()
    {
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) OnLand();
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) OnLaunch();
    }

    private void OnStartStationaryFlight()
    {
        _stationaryFlightActive = true;
        _rb.useGravity = false;
        _rb.linearDamping = 0.95f; // slow down a lot
        _animator.SetBool("flying", true);
        _animator.SetBool("gliding", false);
        _animator.speed = 0.50f;
    }

    private void OnStopStationaryFlight()
    {
        _stationaryFlightActive = false;
        _rb.useGravity = true;
        _rb.linearDamping = 0.00f; // stop slowing down
        _animator.SetBool("flying", false);
        _animator.SetBool("gliding", true);
        _animator.speed = 0.75f;
    }

    private void OnStationaryFlight()
    {
    }
    
    void ResetHopInt()
    {
        _animator.SetInteger("hop", 0);
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _rb.maxLinearVelocity = maxForwardSpeed;
        _rb.maxAngularVelocity = maxForwardSpeed;
        _animator.speed = 0.75f;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * 5;
        float mouseY = Input.GetAxis("Mouse Y") * 5;
        xRotation -= Mathf.Lerp(mouseX, 0, Math.Abs(_rb.linearVelocity.x) * xCameraLock);
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -100f, 70f);
        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);

        HandleInput();
    }

    private void HandleInput()
    {
        // events
        if (Input.GetButtonDown("Jump")) OnStartFlying(); 
        if (Input.GetButtonUp("Jump")) OnStopFlying();
        if (Input.GetKeyDown(KeyCode.LeftShift)) OnStartStationaryFlight();
        if (Input.GetKeyUp(KeyCode.LeftShift)) OnStopStationaryFlight();
        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isOnGround) OnStartLanding();
        if (Input.GetKeyUp(KeyCode.LeftControl)) OnStopLanding();
        if (_wingsActive) OnFlying();

        // static flight directions
        if (Input.GetKey(KeyCode.W)) _staticFlightDirections.Add(transform.forward);
        if (Input.GetKey(KeyCode.A)) _staticFlightDirections.Add(-transform.right);
        if (Input.GetKey(KeyCode.S)) _staticFlightDirections.Add(-transform.forward);
        if (Input.GetKey(KeyCode.D)) _staticFlightDirections.Add(transform.right);
    }
    
    // beware code reviewer, lies below physics code to be reviewed with care. In case of brain aneuryism, call 911 
    void FixedUpdate()
    {
        if (_isLanding)
        {
            transform.position = Vector3.MoveTowards(transform.position, perchPoint, 10 * Time.deltaTime);
        }

        if (IsFlyingWithWings())
        {
            _rb.AddRelativeForce(Vector3.forward * (forwardBias * wingForce * Time.deltaTime), ForceMode.Force);
            _rb.AddForce(Vector3.up * (upBias * wingForce * Time.deltaTime), ForceMode.Force);
        }
        else if (IsStationaryFlightActive())
        {
            ApplyAllForces(_staticFlightDirections);
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
        return !_isOnGround && !_stationaryFlightActive;
    }

    /// <summary>
    /// Allows the bird able to turn even when wings are inactive
    /// </summary>
    private void ApplyTorque()
    {
        // slow down bird in direction of velocity
        _rb.AddForce(
            Vector3.Lerp(Vector3.zero, -_rb.linearVelocity * (torque * Time.deltaTime) / 4,
                Math.Abs(_rb.linearVelocity.x)),
            ForceMode.Force);

        // speed up bird in forward direction
        _rb.AddForce(
            Vector3.Lerp(Vector3.zero, transform.forward * (torque * 5 * Time.deltaTime),
                Math.Abs(_rb.linearVelocity.x)),
            ForceMode.Force);
    }

    private void ApplyAllForces(IEnumerable<Vector3> directions)
    {
        foreach (var staticFlightDirection in _staticFlightDirections)
        {
            _rb.AddForce(staticFlightDirection * (Time.deltaTime * wingForce) / 2, ForceMode.Force);
        }
    }             
}