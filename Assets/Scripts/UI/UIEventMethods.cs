using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
internal class UIEventMethods : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chocolateAmountValue;
    [SerializeField] private TextMeshProUGUI priceAmounPanelValue;
    internal void UpdateChocolateAmount(int value)
    {
        chocolateAmountValue.text = value.ToString();
    }
    internal void UpdatePriceAmount(int value)
    {
        priceAmounPanelValue.text = value.ToString();
    }
}
