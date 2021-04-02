using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StealButton : MonoBehaviour
{

    internal event Action GoSteal;
    private void OnMouseDown()
    {
        GoSteal();
    }
}
