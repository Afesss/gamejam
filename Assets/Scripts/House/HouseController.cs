using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class HouseController : MonoBehaviour
{
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject stealButton;
    [SerializeField] GameObject homeButton;
    [SerializeField] HouseVitality houseVitality;
    private Vector3 targetPosition;

    private void Awake()
    {
        attackButton.GetComponent<AttackButton>().GoAttack += ButtonAttackDown;
        stealButton.GetComponent<StealButton>().GoSteal += ButtonStealDown;
        homeButton.GetComponent<HomeButton>().GoHome += ButtonHomeDown;
        targetPosition = transform.position;
    }
    private void OnMouseDown()
    {
        if (!houseVitality.IsRecieveDamage)
        {
            attackButton.SetActive(true);
            stealButton.SetActive(true);
        }
        else
        {
            homeButton.SetActive(true);
        }
    }
    private void ButtonAttackDown()
    {
        BeaverData.Instance.TargetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.AttackInvoke();
    }
    private void ButtonStealDown()
    {
        BeaverData.Instance.TargetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.StealInvoke();
    }
    private void ButtonHomeDown()
    {
        if (_beaver != null)
        {
            homeButton.SetActive(false);
            _beaver.GoHome(targetPosition);
            _beaver = null;
        }
    }
    private BeaverBehaviour _beaver;
    private void OnTriggerEnter(Collider other)
    {
        _beaver = other.GetComponent<BeaverBehaviour>();
        switch (_beaver.currentState)
        {
            case BeaverBehaviour.State.Attack:
                houseVitality.IsRecieveDamage = true;
                break;
            case BeaverBehaviour.State.Steal:
                Debug.Log("Steals");
                break;
        }
    }
}
