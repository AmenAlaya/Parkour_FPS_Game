using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FirstPersonShooter.Controller
{
    public class Motion : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _sprintModifier;
        [SerializeField] private Camera _normalCam;
        [SerializeField] private Transform _groundDetactor;
        [SerializeField] private LayerMask _groundLayerMask;

        private Rigidbody _myRigidbody;

        private float _fovLerpSpeed = 8f;
        [SerializeField] private float _jumpForce = 1000f;
        private float _baseFov;
        private float _sprintFovModifier = 1.5f;

        //Keys
        private KeyCode _sprintKey = KeyCode.LeftShift;
        private KeyCode _jumpKey = KeyCode.Space;

        private float _hMove;
        private float _vMove;
        private bool _isSprinting;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            _baseFov = _normalCam.fieldOfView;
            _myRigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {

            //Controls
            bool sprint = Input.GetKey(_sprintKey);
            bool jump = Input.GetKeyDown(_jumpKey);


            //States
            Ray ray = new Ray(_groundDetactor.position, Vector3.down);
            bool isGrounded = Physics.Raycast(ray, 0.2f, _groundLayerMask);
            bool isjumping = jump && isGrounded;
            _isSprinting = sprint && _vMove > 0 && !isjumping && isGrounded;


            //Jump
            if (isjumping)
            {
                _myRigidbody.AddForce(Vector3.up * _jumpForce);
            }


        }

        void FixedUpdate()
        {
            //Axis
            _hMove = Input.GetAxisRaw("Horizontal");
            _vMove = Input.GetAxisRaw("Vertical");





            float t_AdjustedSpeed = _speed;
            if (_isSprinting) t_AdjustedSpeed *= _sprintModifier;

            //Movement
            Vector3 t_dir = new Vector3(_hMove, 0, _vMove);
            t_dir.Normalize();

            Vector3 t_targetVelocety = transform.TransformDirection(t_dir) * t_AdjustedSpeed * Time.deltaTime;
            t_targetVelocety.y = _myRigidbody.velocity.y;
            _myRigidbody.velocity = t_targetVelocety;

            //Field Of view
            if (_isSprinting)
            {
                _normalCam.fieldOfView = Mathf.Lerp(_normalCam.fieldOfView, _baseFov * _sprintFovModifier, Time.deltaTime * _fovLerpSpeed);
            }
            else
            {
                _normalCam.fieldOfView = Mathf.Lerp(_normalCam.fieldOfView, _baseFov, Time.deltaTime * _fovLerpSpeed);
            }

        }
    }
    #endregion
}
