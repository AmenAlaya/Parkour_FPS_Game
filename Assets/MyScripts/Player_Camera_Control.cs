using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Player_Camera_Control : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [SerializeField] private Transform _orientation;

    private float _xRot;
    private float _yRot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensY;

        _yRot += mouseX;

        _xRot -= mouseY;
        _xRot = math.clamp(_xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);
        _orientation.rotation = Quaternion.Euler(0, _yRot, 0);

    }

}
