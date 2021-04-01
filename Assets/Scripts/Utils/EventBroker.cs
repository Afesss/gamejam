using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class EventBroker : MonoBehaviour
{
    internal static event Action Attack;
    internal static void AttackInvoke()
    {
        Attack.Invoke();
    }
}
