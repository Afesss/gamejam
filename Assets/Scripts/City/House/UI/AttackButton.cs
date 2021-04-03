using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class AttackButton : MonoBehaviour
{

    internal event Action GoAttack;
    private void OnMouseDown()
    {
        GoAttack();
    }
}
