using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StealButton : MonoBehaviour
{

    internal event Action IsSteal;
    internal bool isSteal { get; set; }
    private void Awake()
    {
    }
    private void OnMouseDown()
    {
        isSteal = true;
        IsSteal();
    }
}
