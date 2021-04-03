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
    private BeaverBehaviour _beaver;

    private void Awake()
    {
        attackButton.GetComponent<AttackButton>().GoAttack += ButtonAttackDown;
        stealButton.GetComponent<StealButton>().GoSteal += ButtonStealDown;
        homeButton.GetComponent<HomeButton>().GoHome += ButtonHomeDown;
        targetPosition = transform.position;
        EventBroker.ButtonDown += HideOtherButtons;
    }
    private void OnDestroy()
    {
        EventBroker.ButtonDown -= HideOtherButtons;
        attackButton.GetComponent<AttackButton>().GoAttack -= ButtonAttackDown;
        stealButton.GetComponent<StealButton>().GoSteal -= ButtonStealDown;
        homeButton.GetComponent<HomeButton>().GoHome -= ButtonHomeDown;
    }
    private void HideOtherButtons()
    {
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        homeButton.SetActive(false);
    }
    private void OnMouseDown()
    {
        EventBroker.ButtonDown -= HideOtherButtons;
        EventBroker.ButtonDownInvoke();
        StopAllCoroutines();
        StartCoroutine(HidingButtons());
        if (!houseVitality.IsRecieveDamage)
        {
            attackButton.SetActive(true);
            stealButton.SetActive(true);
            EventBroker.ButtonDown += HideOtherButtons;
        }
        else
        {
            homeButton.SetActive(true);
            EventBroker.ButtonDown += HideOtherButtons;
        }
    }
    private IEnumerator HidingButtons()
    {
        yield return new WaitForSeconds(1);
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        homeButton.SetActive(false);
    }
    private void ButtonAttackDown()
    {
        BeaversController.targetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.AttackInvoke();
    }
    private void ButtonStealDown()
    {
        BeaversController.targetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.StealInvoke();
    }
    private void ButtonHomeDown()
    {
        if (_beaver != null)
        {
            houseVitality.IsRecieveDamage = false;
            homeButton.SetActive(false);
            _beaver.GoHome(targetPosition);
            _beaver = null;
        }
    }
    
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
