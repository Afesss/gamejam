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
    [SerializeField] HouseChocolate houseChocolate;
    [SerializeField] HouseBeaverDetector houseBeaverDetector;

    private bool isBeaverDetected;

    private Vector3 homeButtonDefaultPosition;
    private Transform homeButtonTransform;
    private Vector3 targetPosition;
    private BeaverBehaviour _beaver;

    private void Awake()
    {
        attackButton.GetComponent<AttackButton>().GoAttack += ButtonAttackDown;
        stealButton.GetComponent<StealButton>().GoSteal += ButtonStealDown;
        homeButton.GetComponent<HomeButton>().GoHome += ButtonHomeDown;
        targetPosition = transform.position;
        homeButtonTransform = homeButton.transform;
        homeButtonDefaultPosition = homeButton.transform.position;
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
        if (!isBeaverDetected)
        {
            EventBroker.ButtonDown -= HideOtherButtons;
            EventBroker.ButtonDownInvoke();
            StopAllCoroutines();
            StartCoroutine(HidingButtons());
        
            if (houseVitality.IsRecieveDamage || houseChocolate.IsStealingActive)
            {
                homeButtonTransform.position = homeButtonDefaultPosition;
                homeButton.SetActive(true);
                EventBroker.ButtonDown += HideOtherButtons;
            }
            else
            {
                attackButton.SetActive(true);
                stealButton.SetActive(true);
                EventBroker.ButtonDown += HideOtherButtons;
            }
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
            if(_beaver.currentState == BeaverBehaviour.State.Steal)
            {
                houseChocolate.OnChocolateOutOfStock -= OnChocolateOutOfStock;
            }
            isBeaverDetected = false;
            houseBeaverDetector.OnDetectBeaver -= OnDetectBeaver;
            houseChocolate.IsStealingActive = false;
            houseVitality.IsRecieveDamage = false;
            homeButton.SetActive(false);
            _beaver.GoHome(targetPosition);
            _beaver = null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _beaver = other.GetComponent<BeaverBehaviour>();
        houseBeaverDetector.OnDetectBeaver += OnDetectBeaver;
        houseBeaverDetector.OnDestroyBeaver += HomeButtonOff;
        switch (_beaver.currentState)
        {
            case BeaverBehaviour.State.Attack:
                houseVitality.IsRecieveDamage = true;
                break;
            case BeaverBehaviour.State.Steal:
                houseChocolate.OnChocolateOutOfStock += OnChocolateOutOfStock;
                houseChocolate.IsStealingActive = true;
                break;
        }
    }

    private void OnChocolateOutOfStock()
    {
        homeButton.SetActive(true);
    }

    private void HomeButtonOff()
    {
        isBeaverDetected = false;
        homeButton.SetActive(false);
        houseBeaverDetector.OnDestroyBeaver -= HomeButtonOff;
    }
    
    private void OnDetectBeaver(Vector3 detectPosition)
    {
        isBeaverDetected = true;
        homeButtonTransform.position = detectPosition;
        homeButton.SetActive(true);
    }
}
