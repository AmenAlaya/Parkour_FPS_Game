using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FirstPersonShooter.Controller
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Gun[] _loadout;
        [SerializeField] private Transform _weaponParent;

        private KeyCode _equipKey = KeyCode.Alpha1;

        private GameObject _currentWeapon;

        void Update()
        {
            if (Input.GetKeyDown(_equipKey))
            {
                Equip(0);
            }
        }
        void Equip(int q_ind)
        {
            if (_currentWeapon != null) return;

            GameObject t_newWeapon = Instantiate(_loadout[q_ind].prefab, _weaponParent.position, _weaponParent.rotation, _weaponParent) as GameObject;
            t_newWeapon.transform.localPosition = Vector3.zero;
            t_newWeapon.transform.localEulerAngles = Vector3.zero;

            _currentWeapon = t_newWeapon;
        }
    }
}
