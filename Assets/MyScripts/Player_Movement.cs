using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    private float _movementSpeed;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;

    [SerializeField]
    private float _slideSpeed;

    private float _desiredMoveSpeed;
    private float _lastDesiredMoveSpeed;

    [SerializeField] private float _speedIncMultiplier;
    [SerializeField] private float _slopeIncMultiplier;

    [SerializeField] private float groundDrag;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMuliplier;
    private bool _canJump = true;

    [Header("Crouch Check")]
    [SerializeField] private float _crouchSpeed;

    [SerializeField] private float _crouchYScale;
    private float _startYScale;

    [Header("Slope Handling")]
    [SerializeField] private float _maxScopeAngle;

    private RaycastHit _slopeHit;
    private bool _isSloping;

    [Header("Keysinds")]
    private KeyCode _jumpKey = KeyCode.Space;

    private KeyCode _sprintKey = KeyCode.LeftShift;
    private KeyCode _crouchKey = KeyCode.C;

    [Header("Ground Check")]
    [SerializeField] private float _playerHieght;

    [SerializeField] private LayerMask _whatIsGround;
    private bool _isGrounded = true;

    [SerializeField] private Transform _orientation;

    private float _horizantalInput;
    private float _verticalInput;

    private Vector3 _moveDir;

    private Rigidbody _myRb;

    private Movement_Sate _myState;

    public enum Movement_Sate
    {
        WALKING,
        SPRINTING,
        CROUCHING,
        SLIDING,
        AIR
    }

    public bool isSliding;

    private void Start()
    {
        _myRb = GetComponent<Rigidbody>();
        _myRb.freezeRotation = true;
        _startYScale = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        Move_Player();
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHieght * .5f + .2f, _whatIsGround);

        My_Input();
        Speed_Control();
        Sate_Handeler();

        Check_Ground(_isGrounded);

        _myRb.useGravity = !On_Slope();
    }

    private void Check_Ground(bool isGrounded)
    {
        if (isGrounded)
            _myRb.drag = groundDrag;
        else
            _myRb.drag = 0;
    }

    private void My_Input()
    {
        _horizantalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(_jumpKey) && _canJump && _isGrounded)
        {
            _canJump = false;
            Jump();

            Invoke(nameof(jump_Sate), _jumpCooldown);
        }

        if (Input.GetKeyDown(_crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);
            _myRb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(_crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
        }
    }

    private void Sate_Handeler()
    {
        if (isSliding)
        {
            _myState = Movement_Sate.SLIDING;

            if (On_Slope() && _myRb.velocity.y < .1f)
            {
                _desiredMoveSpeed = _slideSpeed;
            }
            else
            {
                _desiredMoveSpeed = _sprintSpeed;
            }
        }
        else if (Input.GetKey(_crouchKey))
        {
            _myState = Movement_Sate.CROUCHING;
            _desiredMoveSpeed = _crouchSpeed;
        }
        else if (_isGrounded && Input.GetKey(_sprintKey))
        {
            _myState = Movement_Sate.SPRINTING;
            _desiredMoveSpeed = _sprintSpeed;
        }
        else if (_isGrounded)
        {
            _myState = Movement_Sate.WALKING;
            _desiredMoveSpeed = _walkSpeed;
        }
        else
        {
            _myState = Movement_Sate.AIR;
        }

        if (Math.Abs(_desiredMoveSpeed - _lastDesiredMoveSpeed) > 4f && _movementSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(Smoothly_Lerp_Move_Speed());
        }
        else
        {
            _movementSpeed = _desiredMoveSpeed;
        }

        _lastDesiredMoveSpeed = _desiredMoveSpeed;
    }

    private IEnumerator Smoothly_Lerp_Move_Speed()
    {
        float time = 0;
        float difference = Mathf.Abs(_desiredMoveSpeed - _movementSpeed);
        float startValue = _movementSpeed;

        while (time < difference)
        {
            _movementSpeed = Mathf.Lerp(startValue, _desiredMoveSpeed, time / difference);

            if (On_Slope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90);
                time += Time.deltaTime * _speedIncMultiplier * _slopeIncMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * _speedIncMultiplier;
            }

            yield return null;
        }
        _movementSpeed = _desiredMoveSpeed;
    }

    private void Move_Player()
    {
        //calculate movement direction
        _moveDir = _orientation.forward * _verticalInput + _orientation.right * _horizantalInput;

        //On Slope

        if (On_Slope() && !_isSloping)
        {
            _myRb.AddForce(Get_Splope_Move_Direction(_moveDir) * _movementSpeed * 20, ForceMode.Force);

            if (_myRb.velocity.y > 0) _myRb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (_isGrounded)
            _myRb.AddForce(_moveDir * _movementSpeed * 10f, ForceMode.Force);
        else
            _myRb.AddForce(_moveDir * _movementSpeed * 10f * _airMuliplier, ForceMode.Force);
    }

    private void Speed_Control()
    {
        if (On_Slope() && !_isSloping)
        {
            if (_myRb.velocity.magnitude > _movementSpeed)
                _myRb.velocity = _myRb.velocity.normalized * _movementSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(_myRb.velocity.x, 0f, _myRb.velocity.z);

            if (flatVel.magnitude > _movementSpeed)
            {
                Vector3 limitVel = flatVel.normalized * _movementSpeed;
                _myRb.velocity = new Vector3(limitVel.x, _myRb.velocity.y, limitVel.z);
            }
        }
    }

    private void Jump()
    {
        _isSloping = true;
        //Reset velocity
        _myRb.velocity = new Vector3(_myRb.velocity.x, 0f, _myRb.velocity.z);

        _myRb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void jump_Sate()
    {
        _isSloping = false;
        this._canJump = true;
    }

    public bool On_Slope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHieght * .5f + .3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxScopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 Get_Splope_Move_Direction(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, _slopeHit.normal).normalized;
    }
}