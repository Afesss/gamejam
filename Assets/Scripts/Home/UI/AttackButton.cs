using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class AttackButton : MonoBehaviour
{

    internal event Action IsAttack;
    internal bool isAttack { get; set; }
    private void Awake()
    {
    }
    private void OnMouseDown()
    {
        isAttack = true;
        IsAttack();
    }
}
