using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FirstPersonShooter.Controller
{
    public class Motion : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _sprintModifier;
        [SerializeField] private Camera _normalCam;



        private Rigidbody _myRigidbody;

        public float _fovLerpSpeed;
        private float _baseFov;
        private float _sprintFovModifier = 1.5f;
        private KeyCode _sprintKey = KeyCode.LeftShift;

        void Start()
        {
            _baseFov=_normalCam.fieldOfView;
            _myRigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float t_hMove = Input.GetAxisRaw("Horizontal");
            float t_vMove = Input.GetAxisRaw("Vertical");

            bool sprint = Input.GetKey(_sprintKey);

            bool isSprinting = sprint && t_vMove > 0;

            float t_AdjustedSpeed = _speed;
            if (isSprinting) t_AdjustedSpeed *= _sprintModifier;


            Vector3 t_dir = new Vector3(t_vMove, 0, t_hMove);
            t_dir.Normalize();

            _myRigidbody.velocity = transform.TransformDirection(t_dir) * t_AdjustedSpeed * Time.deltaTime;

            if (isSprinting)
            {
                _normalCam.fieldOfView =Mathf.Lerp(_normalCam.fieldOfView, _baseFov * _sprintFovModifier,Time.deltaTime*_fovLerpSpeed);
            }
            else
            {
                _normalCam.fieldOfView = Mathf.Lerp(_normalCam.fieldOfView, _baseFov, Time.deltaTime * _fovLerpSpeed);
            }

        }
    }
}
