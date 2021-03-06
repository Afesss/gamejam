using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private AudioSource menuThemeAudio = null;
    [SerializeField] private AudioSource mainThemeAudio = null;
    private Transform _transform;
    private Vector3 rotateAngle;
    private float xAxis;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _transform = transform;
        EventBroker.SendWaterPosition += ChengePosition;
    }
    private void Start()
    {
        UIManager.Instance.OnUpdateGameState += ToggleAudioListener;
    }
    private void ToggleAudioListener()
    {
        if (GameManager.Instance.currentGameState == GameManager.GameState.RUNNING)
        {
            menuThemeAudio.Stop();
            mainThemeAudio.Play();
        }
        else
        {
            menuThemeAudio.Play();
            mainThemeAudio.Stop();
        }
    }

    private void OnDestroy()
    {
        EventBroker.SendWaterPosition -= ChengePosition;
        if (UIManager.Instance != null)
            UIManager.Instance.OnUpdateGameState -= ToggleAudioListener;
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
    private Vector3 newPosition;
    private void ChengePosition(float yPos)
    {
        newPosition = _transform.position;
        newPosition.y = yPos;
        _transform.position = newPosition;
    }
}
