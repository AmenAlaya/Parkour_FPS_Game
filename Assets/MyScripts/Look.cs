using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FirstPersonShooter.Controller
{
    public class Look : MonoBehaviour
    {
        #region Variables
        public static bool cursorLocked = true;

        [SerializeField] private Transform _player;
        [SerializeField] private Transform _cams;
        [SerializeField] private Transform _weapon;

        [SerializeField] private float _xSensivity;
        [SerializeField] private float _ySensivity;
        [SerializeField] private float _maxAngle;

        private Quaternion _camCenter;
        #endregion

        #region MonoBhaviour CallBacks
        private void Start()
        {
            _camCenter = _cams.localRotation;
        }

        void Update()
        {
            Set_Y();
            Set_X();

            Update_Cusor_Locked();
        }
        #endregion

        #region Private Methods
        void Set_X()
        {
            float t_input = Input.GetAxis("Mouse X") * _xSensivity * Time.deltaTime;
            Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
            Quaternion t_deta = _player.localRotation * t_adj;
            _player.localRotation = t_deta;

        }

        void Set_Y()
        {
            float t_input = Input.GetAxis("Mouse Y") * _ySensivity * Time.deltaTime;
            Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
            Quaternion t_deta = _cams.localRotation * t_adj;

            if (Quaternion.Angle(_camCenter, t_deta) < _maxAngle)
            {
                _cams.localRotation = t_deta;
            }

            _weapon.rotation = _cams.rotation;

        }

        void Update_Cusor_Locked()
        {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                cursorLocked = true;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = true;
                }
            }
        }
    }
    #endregion
}
