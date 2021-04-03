using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class EventBroker : MonoBehaviour
{
    internal static event Action Attack;
    internal static event Action Steal;
    internal static event Action GoHome;
    internal static event Action ButtonDown;

    public delegate void WaterFlowAction(float amount);
    public static event WaterFlowAction OnHouseWaterFlow;


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
    internal static void ButtonDownInvoke()
    {
        ButtonDown.Invoke();
    }
    internal static void OnHouseWaterFlowInvoke(float amount)
    {
        OnHouseWaterFlow?.Invoke(amount);
    }
}
