using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// ��������� ������ ���� ����������� ����� ��������� :)
/// </summary>
public class HouseChocolate : MonoBehaviour
{
    public bool IsStealingActive { get { return isStealingActive; } set { isStealingActive = value; } }

    public delegate void StealAction(int remain);
    public event StealAction OnChocolateSteal;
    public event Action OnChocolateOutOfStock;

    [Tooltip("������ �� ����� �������")]
    [SerializeField]
    private bool isStealingActive;

    [Tooltip("��������� ����")]
    [SerializeField]
    private HouseSettings config;

    private float currentAmount;
    private int currentIntAmount;
    internal int CurrentIntAmount { get { return currentIntAmount; } }
    private HouseController houseController;
    internal bool chocolateEmpty { get; private set; }

    private void Awake()
    {
        currentIntAmount = UnityEngine.Random.Range(config.MinChocolateAmount, config.MaxChocolateAmount);
        currentAmount = currentIntAmount + 1;
        houseController = GetComponent<HouseController>();
        chocolateEmpty = false;
    }

    void Update()
    {
        
        if(currentIntAmount == 0)
        {
            chocolateEmpty = true;
            houseController.ResetHomeButtonPosition();
            houseController.HomeButton.SetActive(true);
            houseController.ChocolateSign.SetActive(false);
            currentIntAmount = -1;
        }
        if (isStealingActive && currentIntAmount > 0)
        {
            currentAmount -= config.�hocolateStealPerSecond * Time.deltaTime;
            if (currentAmount <= currentIntAmount)
            {
                currentIntAmount--;
                if (currentIntAmount > 0)
                    OnChocolateSteal?.Invoke(currentIntAmount);
                else
                    OnChocolateOutOfStock?.Invoke();
            }
            
        }
    }
    internal void ReturnStealdChocolate(int amount)
    {
        currentIntAmount += amount;
    }
}