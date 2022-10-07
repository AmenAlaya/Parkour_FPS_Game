using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Pos : MonoBehaviour
{
    [SerializeField] private Transform _CamPos;

    void Update()
    {
        transform.position= _CamPos.position ;
    }
}
