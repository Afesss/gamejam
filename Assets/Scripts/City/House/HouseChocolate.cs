using System;
using UnityEngine;

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

    private void Awake()
    {
        currentIntAmount = UnityEngine.Random.Range(config.MinChocolateAmount, config.MaxChocolateAmount);
        currentAmount = currentIntAmount + 1;
    }

    void Update()
    {
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