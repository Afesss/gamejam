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
    internal static event Action UpdateGUI;
    internal static event Action GameOver;
    internal static event Action<int> UpdateChocolateAmount;
    internal static event Action<int> UpdatePriceAmoun;
    internal static event Action BuyBeaver;

    internal delegate void WaterFlowAction(float amount);
    internal static event WaterFlowAction OnHouseWaterFlow;

    internal delegate void StealAction(float remain);
    internal static event StealAction OnChocolateSteal;
    internal static event Action OnChocolateOutOfStock;
    internal static void BuyBeaverInvoke()
    {
        BuyBeaver.Invoke();
    }
    internal static void UpdatePriceInvoke(int value)
    {
        UpdatePriceAmoun.Invoke(value);
    }
    internal static void UpdateChocolateInvoke(int value)
    {
        UpdateChocolateAmount.Invoke(value);
    }

    internal static void GameOverInvoke()
    {
        GameOver.Invoke();
    }
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
    internal static void UpdateGUIInvoke()
    {
        UpdateGUI.Invoke();
    }
    internal static void OnHouseWaterFlowInvoke(float amount)
    {
        OnHouseWaterFlow?.Invoke(amount);
    }

    internal static void OnChocolateStealInvoke(float remain)
    {
        OnChocolateSteal?.Invoke(remain);
    }

    internal static void OnChocolateOutOfStockInvoke()
    {
        OnChocolateOutOfStock?.Invoke();
    }

}