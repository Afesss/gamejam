using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal class HouseController : MonoBehaviour
{
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject stealButton;
    [SerializeField] GameObject homeButton;
    [SerializeField] GameObject chocolateSign;
    [SerializeField] GameObject toiletSign;
    [SerializeField] HouseVitality houseVitality;
    [SerializeField] HouseChocolate houseChocolate;
    [SerializeField] HouseBeaverDetector houseBeaverDetector;
    internal GameObject HomeButton { get { return homeButton; } }
    internal GameObject ChocolateSign { get { return chocolateSign; } }
    private bool isBeaverDetected;
    private bool beaverMoveToHouse;

    private Vector3 homeButtonDefaultPosition;
    private Vector3 targetPosition;

    internal Transform homeButtonTransform;
    private BeaverBehaviour _beaver;
    internal Animation _animation { get; private set; }
    private Collider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _animation = GetComponent<Animation>();
        _animation.Stop();
        attackButton.GetComponent<AttackButton>().GoAttack += ButtonAttackDown;
        stealButton.GetComponent<StealButton>().GoSteal += ButtonStealDown;
        homeButton.GetComponent<HomeButton>().GoHome += ButtonHomeDown;
        targetPosition = transform.position;
        homeButtonTransform = homeButton.transform;
        homeButtonDefaultPosition = homeButton.transform.position;
        EventBroker.ButtonDown += HideOtherButtons;
    }
    internal void ResetHomeButtonPosition()
    {
        homeButtonTransform.position = homeButtonDefaultPosition;
    }
    private void OnDestroy()
    {
        EventBroker.ButtonDown -= HideOtherButtons;
        attackButton.GetComponent<AttackButton>().GoAttack -= ButtonAttackDown;
        stealButton.GetComponent<StealButton>().GoSteal -= ButtonStealDown;
        homeButton.GetComponent<HomeButton>().GoHome -= ButtonHomeDown;
    }
    
    private void OnMouseDown()
    {
        if (!isBeaverDetected && !beaverMoveToHouse)
        {
            EventBroker.ButtonDown -= HideOtherButtons;
            EventBroker.ButtonDownInvoke();
            StopAllCoroutines();
            StartCoroutine(HidingButtons());
        
            if (houseVitality.IsRecieveDamage || houseChocolate.IsStealingActive)
            {
                ResetHomeButtonPosition();
                homeButton.SetActive(true);
                EventBroker.ButtonDown += HideOtherButtons;
            }
            else
            {
                if (BeaversController.CountBeaverInQueue != 0)
                {
                    if(!houseVitality.IsFlooded)
                        attackButton.SetActive(true);
                    if (!houseChocolate.chocolateEmpty)
                    {
                        stealButton.SetActive(true);
                    }
                    EventBroker.ButtonDown += HideOtherButtons;
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        EventBroker.SendWaterPosition += WaterUP;
        beaverMoveToHouse = false;
        _beaver = other.GetComponent<BeaverBehaviour>();
        houseBeaverDetector.OnDetectBeaver += OnDetectBeaver;
        houseBeaverDetector.OnDestroyBeaver += HomeButtonOff;
        switch (_beaver.currentState)
        {
            case BeaverBehaviour.State.Attack:
                houseVitality.IsRecieveDamage = true;
                houseVitality.PlayDamageRecievedAudio();
                toiletSign.SetActive(true);
                break;
            case BeaverBehaviour.State.Steal:
                houseChocolate.OnChocolateOutOfStock += OnChocolateOutOfStock;
                houseChocolate.IsStealingActive = true;
                chocolateSign.SetActive(true);
                break;
        }
    }
    private void HideOtherButtons()
    {
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        //homeButton.SetActive(false);
    }
    private IEnumerator HidingButtons()
    {
        yield return new WaitForSeconds(4);
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        //homeButton.SetActive(false);
    }
    
    private void ButtonAttackDown()
    {
        beaverMoveToHouse = true;
        BeaversController.targetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.AttackInvoke();
    }
    private void ButtonStealDown()
    {
        beaverMoveToHouse = true;
        BeaversController.targetPosition = targetPosition;
        attackButton.SetActive(false);
        stealButton.SetActive(false);
        EventBroker.StealInvoke();
    }
    private void ButtonHomeDown()
    {
        if (_beaver != null)
        {
            if (_beaver.currentState == BeaverBehaviour.State.Steal)
            {
                houseChocolate.OnChocolateOutOfStock -= OnChocolateOutOfStock;
            }
            if (isBeaverDetected)
            {
                _animation.Stop();
                isBeaverDetected = false;
            }
            chocolateSign.SetActive(false);
            toiletSign.SetActive(false);
            houseBeaverDetector.OnDetectBeaver -= OnDetectBeaver;
            houseChocolate.IsStealingActive = false;
            houseVitality.IsRecieveDamage = false;
            homeButton.SetActive(false);
            _beaver.GoHome(targetPosition);
            _beaver = null;
            _collider.enabled = true;
        }
    }
    private void OnChocolateOutOfStock()
    {
        homeButton.SetActive(true);
    }

    private void HomeButtonOff()
    {
        _animation.Stop();
        isBeaverDetected = false;
        homeButton.SetActive(false);
        houseBeaverDetector.OnDestroyBeaver -= HomeButtonOff;
    }
    
    private void OnDetectBeaver(Vector3 detectPosition)
    {
        _collider.enabled = false;
        chocolateSign.SetActive(false);
        toiletSign.SetActive(false);
        _animation.Play();
        isBeaverDetected = true;
        homeButtonTransform.position = detectPosition;
        homeButton.SetActive(true);
    }
    private void WaterUP(float xPos)
    {
        if (_beaver != null)
        {
            if (homeButton.transform.position.y < xPos)
            {
                HomeButtonOff();
                _beaver.Die();
                EventBroker.SendWaterPosition -= WaterUP;
            }
        }
    }
}
