using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstPersonShooter.Controller
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public string name;
        public float damage;
        public float aimSpeed;
        public int fireRate;
        public GameObject prefab;

    }
}
