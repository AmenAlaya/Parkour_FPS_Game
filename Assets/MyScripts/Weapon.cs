using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

namespace FirstPersonShooter.Controller
{
    public class Weapon : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Gun[] _loadout;
        [SerializeField] private Transform _weaponParent;

        private KeyCode _equipKey = KeyCode.Alpha1;

        private int _currentindex;
        private GameObject _currentWeapon;


        #endregion

        #region MonoBehaviour CallBacks


        void Update()
        {

            if (Input.GetKeyDown(_equipKey)) Equip(0);
            Aim(Input.GetMouseButton(1));
        }


        #endregion


        #region Private Methods

        void Equip(int p_ind)
        {
            if (_currentWeapon != null) return;

            GameObject t_newWeapon = Instantiate(_loadout[p_ind].prefab, _weaponParent.position, _weaponParent.rotation, _weaponParent) as GameObject;
            t_newWeapon.transform.localPosition = Vector3.zero;
            t_newWeapon.transform.localEulerAngles = Vector3.zero;

            _currentindex = p_ind;
            _currentWeapon = t_newWeapon;
        }

        void Aim(bool p_isAiming)
        {
            if (_currentWeapon != null) return;

            Transform t_Anchor = _currentWeapon.transform.Find("Anchor");
            Transform t_adsState = _currentWeapon.transform.Find("Sates/ADS");
            Transform t_hipState = _currentWeapon.transform.Find("Sates/Hip");

            if (p_isAiming)
            {
                //Aim
                t_Anchor.position = Vector3.Lerp(t_Anchor.position, t_adsState.position, Time.deltaTime * _loadout[_currentindex].aimSpeed);
            }
            else
            {
                //Hip
                t_Anchor.position = Vector3.Lerp(t_Anchor.position, t_hipState.position, Time.deltaTime * _loadout[_currentindex].aimSpeed);
            }
        }


        #endregion
    }
}
