using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AttackButton : MonoBehaviour
{
    [SerializeField] private GameObject stealButton;
    private GameObject _gameObject;
    private void Awake()
    {
        _gameObject = gameObject;
    }
    private void OnMouseDown()
    {
        EventBroker.AttackInvoke();
        _gameObject.SetActive(false);
        stealButton.SetActive(false);
    }
}
