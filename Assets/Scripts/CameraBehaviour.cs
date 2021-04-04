using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private float rotateSpeed;
    private Transform _transform;
    private Vector3 rotateAngle;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _transform = transform;
    }
    private void FixedUpdate()
    {
        CameraRotate();
    }
    private void CameraRotate()
    {
        rotateAngle.y += -_input.horizontalInput * rotateSpeed * Time.deltaTime;
        rotateAngle.x += _input.verticalInput * rotateSpeed * Time.deltaTime;
       
        _transform.SetPositionAndRotation(_transform.position, Quaternion.Euler(rotateAngle));
    }
}
