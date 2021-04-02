using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HomeButton : MonoBehaviour
{
    internal event Action GoHome;
    private void OnMouseDown()
    {
        GoHome();
    }
}
