using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class HomeController : MonoBehaviour
{
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject stealButton;
    private void Awake()
    {
    }
    private void OnMouseDown()
    {
        attackButton.SetActive(true);
        stealButton.SetActive(true);
    }
}
