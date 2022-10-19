using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FirstPersonShooter.Controller
{
    public class Sway : MonoBehaviour
    {
        #region Variables


        [SerializeField] private float _intensity;
        [SerializeField] private float _smooth;

        private Quaternion _originRot;


        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            _originRot = transform.localRotation;
        }


        void Update()
        {
            Update_Sway();
        }


        #endregion


        #region Private Methods

        void Update_Sway()
        {
            //Controls
            float t_xMouse = Input.GetAxis("Mouse X");
            float t_yMouse = Input.GetAxis("Mouse Y");

            //Calculate target rotation

            Quaternion t_xadj = Quaternion.AngleAxis(-_intensity * t_xMouse, Vector3.up);
            Quaternion t_yadj = Quaternion.AngleAxis(_intensity * t_yMouse, Vector3.right);
            Quaternion targetRot = _originRot * t_xadj * t_yadj;

            //Rotate gun into target rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * _smooth);
        }


        #endregion
    }
}
