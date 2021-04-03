using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        EventBroker.BuyBeaverInvoke();
        Debug.Log("+");
    }
}
