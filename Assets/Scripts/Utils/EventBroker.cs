using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class EventBroker : MonoBehaviour
{
    internal static event Action Attack;
    internal static event Action Steal;
    internal static event Action GoHome;
    internal static void AttackInvoke()
    {
        Attack.Invoke();
    }
    internal static void StealInvoke()
    {
        Steal.Invoke();
    }
    internal static void GoHomeInvoke()
    {
        GoHome.Invoke();
    }
}
