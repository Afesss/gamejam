using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InputController : MonoBehaviour
{
    internal float horizontalInput { get; private set; }
    internal float verticalInput { get; private set; }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
