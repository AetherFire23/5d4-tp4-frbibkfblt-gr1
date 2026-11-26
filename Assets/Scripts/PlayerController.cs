using System;
using System.Collections.Generic;
using SaveSystem;
using UnityEngine;

public class NoclipPlayerController : MonoBehaviour, IUniqueObject
{
    public float maxForwardSpeed = 500;
    public float wingForce = 6700f;
    public float upBias = 0.6f;
    public float forwardBias = 0.7f;
    public float torque = 10000f;
    public float xCameraLock = 0.0f;
    public float baseLinearDamping = 0.05f;

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
    private float previousXRotation = 0f;
    private float previousYRotation = 0f;
    private BirdInventory _birdInventory;
    private BirdTrailController _birdTrailController;

    private Rigidbody _rb;
    private AudioSource _windNoise;

    private void OnLand()
    {
        // animations
        _animator.SetBool("gliding", false);
        _animator.SetBool("flying", _wingsActive);
        _animator.SetBool("landing", false);
        _birdTrailController.StopTrail();

        // physics
        _isOnGround = true;
        _isLanding = false;
        _rb.linearDamping = 0.5f;
        _rb.linearVelocity /= 10;
        _rb.mass = 25;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
    }

    private void OnStartFlying()
    {
        _rb.constraints = RigidbodyConstraints.None;
        _animator.SetBool("flying", true);
        _animator.SetBool("gliding", false);
        torque /= 5;
        if (_isOnGround)
        {
            _animator.SetInteger("hop", 1);
            _rb.AddForce(transform.up * (wingForce * Time.deltaTime * 0.5f),
                ForceMode.Impulse); // small jump to kickstart flight
            _rb.AddForce(transform.forward * (wingForce * Time.deltaTime * 1f),
                ForceMode.Impulse); // small jump to kickstart flight
            StartCoroutine(nameof(ResetHopInt), 2);
        }
        else
        {
            _wingsActive = true;
        }
    }

    private void OnStartGliding()
    {
        _rb.maxLinearVelocity *= 2;
        _birdTrailController.Trail();
    }

    private void OnStopFlying()
    {
        if (!_isOnGround)
        {
            _animator.SetBool("gliding", true);
        }

        _animator.SetBool("flying", false);
        _wingsActive = false;
        torque *= 5;
    }

    private void OnStopGliding()
    {
        _rb.maxLinearVelocity = maxForwardSpeed;
        _birdTrailController.StopTrail();
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
        _windNoise.Play();
        _isOnGround = false;
        _rb.linearDamping = baseLinearDamping;
        _rb.mass = 10;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;
    }

    private void OnFlying()
    {
        _windNoise.pitch = _rb.linearVelocity.magnitude / 20;
        _windNoise.volume = _rb.linearVelocity.magnitude / 30;
        _staticFlightDirections.Add(transform.up);
    }

    private void OnGliding()
    {
        _windNoise.pitch = _rb.linearVelocity.magnitude / 20;
        _windNoise.volume = _rb.linearVelocity.magnitude / 30;
        _birdTrailController.SetTrailTime(Vector3.Dot(_rb.linearVelocity, transform.forward)/10);
    }

    void OnCollisionEnter(Collision collision)
    {
        print(_rb.linearVelocity.magnitude);
        if (_rb.linearVelocity.magnitude > 15f)
        {
            print("drop");
            _birdInventory.StopGrabbing();
        }

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
        _rb.linearDamping = baseLinearDamping; // stop slowing down
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
        _wingsActive = true; 
    }

    private void Awake()
    {
        Persistance.RegisterPlayer(this.gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _rb.maxLinearVelocity = maxForwardSpeed;
        _rb.maxAngularVelocity = maxForwardSpeed;
        _animator.speed = 0.75f;
        _birdInventory = GetComponent<BirdInventory>();
        _birdTrailController = GetComponent<BirdTrailController>();
        _windNoise = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * 5;
        float mouseY = Input.GetAxis("Mouse Y") * 5;
        xRotation -= Mathf.Lerp(mouseX, 0, Math.Abs(_rb.linearVelocity.x) * xCameraLock);
        yRotation -= mouseY;

        bool near = IsOnGroundOrNear();
        yRotation = Mathf.Clamp(yRotation, near ? -35 : -90f, near ? 10 : 70f);
        print(yRotation);
        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);
        HandleInput();
    }

    private void HandleInput()
    {
        // events
        if (Input.GetButtonDown("Jump"))
        {
            if (!IsOnGroundOrNear())
            {
                OnStopGliding();
            }
            OnStartFlying();
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (!IsOnGroundOrNear()) OnStartGliding();
            OnStopFlying();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnStopGliding();
            OnStartStationaryFlight();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) OnStopStationaryFlight();
        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isOnGround)
        {
            OnStopGliding();
            OnStartLanding();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl)) OnStopLanding();
        if (_wingsActive) OnFlying();
        else if (!_isOnGround) OnGliding();
        if (!_isOnGround)

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
            _rb.AddForce(Vector3.up * (upBias * wingForce / 2 * Time.deltaTime), ForceMode.Force);
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
        previousXRotation = xRotation;
        previousYRotation = yRotation;
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

    private bool IsTurning()
    {
        return (Math.Abs(previousXRotation - xRotation) > 0.1f || previousYRotation - yRotation > 0.0f);
    }

    /// <summary>
    /// Allows the bird able to turn even when wings are inactive
    /// </summary>
    private void ApplyTorque()
    {
        float slowdownMultiplier = IsTurning() ? 1.25f : 0.350f;
        // slow down bird in direction of velocity
        _rb.AddForce(-_rb.linearVelocity * (torque / 8 * slowdownMultiplier * Time.deltaTime *
                                            Mathf.Clamp(
                                                Mathf.Abs(transform.InverseTransformDirection(_rb.linearVelocity)
                                                    .x), 0f, 1f)
            ), ForceMode.Force);

        // speed up bird in forward direction
        _rb.AddForce(transform.forward * (torque * 2.5f * slowdownMultiplier * Time.deltaTime *
                                          Mathf.Clamp(
                                              Mathf.Abs(transform.InverseTransformDirection(_rb.linearVelocity).x), 0,
                                              1f)
            ), ForceMode.Force);
    }

    private void ApplyAllForces(IEnumerable<Vector3> directions)
    {
        foreach (var staticFlightDirection in directions)
        {
            _rb.AddForce(staticFlightDirection * (Time.deltaTime * wingForce) / 2, ForceMode.Force);
        }
    }

    private class PlayerObjectData : ObjectData
    {
        public bool isOnGround;

        public PlayerObjectData(Transform transform, bool isOnGround) : base(transform)
        {
            this.isOnGround = isOnGround;
            Id = "Player";
        }
    }

    public ObjectData GetObjectData()
    {
        return new PlayerObjectData(transform, _isOnGround);
    }

    public bool IsOnGroundOrNear()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hit.distance < 0.3f)
        {
            return true;
        }

        return false;
    }

    public void BuildFromObjectData(ObjectData objectData)
    {
        PlayerObjectData playerObjectData = objectData as PlayerObjectData;
        transform.position = objectData.Position;
        transform.rotation = new Quaternion(objectData.Rotation.x, objectData.Rotation.y, objectData.Rotation.z, 0);
        transform.localScale = objectData.Scale;
        _isOnGround = playerObjectData.isOnGround;
    }
}