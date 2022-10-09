using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("Refeences")]
    [SerializeField] private Transform _orientation;

    [SerializeField] private Transform _playerObj;
    private Rigidbody _rb;
    private Player_Movement _playerMovement;

    [Header("Sliding")]
    [SerializeField] private float _maxSlideTime;

    private float _sliderTimer;

    [SerializeField] private float _slideForce;

    private float _slideYScale = .5f;
    private float _startYScale;

    [Header("Input")]
    private KeyCode _slideKey = KeyCode.LeftControl;

    private float _horizontalInput;
    private float _verticalInput;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<Player_Movement>();
        _startYScale = _playerObj.localScale.y;
    }

    private void FixedUpdate()
    {
        if (_playerMovement.isSliding) Slide_Movement();
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey) && (_horizontalInput != 0 || _verticalInput != 0)) Start_Slide();

        if (Input.GetKeyUp(_slideKey) && _playerMovement.isSliding) Stop_Slide();
    }

    private void Start_Slide()
    {
        _playerMovement.isSliding = true;

        _playerObj.localScale = new Vector3(_playerObj.localScale.x, _slideYScale, _playerObj.localScale.z);
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        _sliderTimer = _maxSlideTime;
    }

    private void Slide_Movement()
    {
        Vector3 inputDir = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        if (_playerMovement.On_Slope() && _rb.velocity.z > -.1f)
        {
            _rb.AddForce(inputDir * _slideForce, ForceMode.Force);

            _sliderTimer -= Time.deltaTime;
        }
        else
        {
            _rb.AddForce(_playerMovement.Get_Splope_Move_Direction(inputDir) * _slideForce, ForceMode.Force);
        }
        if (_sliderTimer <= 0) Stop_Slide();
    }

    private void Stop_Slide()
    {
        _playerMovement.isSliding = false;

        _playerObj.localScale = new Vector3(_playerObj.localScale.x, _startYScale, _playerObj.localScale.z);
    }
}