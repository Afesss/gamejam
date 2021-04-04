using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private float rotateSpeed;
    private Transform _transform;
    private Vector3 rotateAngle;
    private float xAxis;
    
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
        xAxis = xAxis < -30 ? -30 : xAxis;
        xAxis = xAxis > 0 ? 0 : xAxis;
        xAxis += _input.verticalInput * rotateSpeed * Time.deltaTime;

        rotateAngle.x = Mathf.Clamp(xAxis, -30, 0);
        _transform.SetPositionAndRotation(_transform.position, Quaternion.Euler(rotateAngle));
    }
}
