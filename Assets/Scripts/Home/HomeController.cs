using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class HomeController : MonoBehaviour
{
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject stealButton;
    private Transform _transform;
    private Vector3 targetPosition;
    private void Awake()
    {
        attackButton.GetComponent<AttackButton>().IsAttack += ButtonDown;
        stealButton.GetComponent<StealButton>().IsSteal += ButtonDown;
        targetPosition = transform.position;
    }
    private void OnMouseDown()
    {
        attackButton.SetActive(true);
        stealButton.SetActive(true);
    }
    private void ButtonDown()
    {
        BeaverData.Instance.TargetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.AttackInvoke();
    }
}
