using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealButton : MonoBehaviour
{
    [SerializeField] private GameObject attackButton;
    private GameObject _gameObject;
    private void Awake()
    {
        _gameObject = gameObject;
    }
    private void OnMouseDown()
    {
        _gameObject.SetActive(false);
        attackButton.SetActive(false);
    }
}
