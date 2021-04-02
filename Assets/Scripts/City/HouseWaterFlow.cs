using UnityEngine;

[RequireComponent(typeof(HouseVitality))]
public class HouseWaterFlow : MonoBehaviour
{
    [Tooltip("��������� ����")]
    [SerializeField]
    private HouseSettings config;

    [Tooltip("��������� ������ �������������� ������")]
    [SerializeField]
    private HouseDamageLevel damageLevelComponent;

    private int currentDamageLevel = 0;

    private void Awake()
    {
        damageLevelComponent.OnLevelIncrease += OnDamageLevelUpdate;
        damageLevelComponent.OnLevelDecrease += OnDamageLevelUpdate;
    }

    private void Update()
    {
        if (currentDamageLevel > 0)
            EventBroker.OnHouseWaterFlowInvoke(currentDamageLevel * config.WaterFlowAmountByLevel);
    }

    private void OnDamageLevelUpdate(int level)
    {
        currentDamageLevel = level;
    }

    private void OnDestroy()
    {
        if (damageLevelComponent != null)
        {
            damageLevelComponent.OnLevelIncrease -= OnDamageLevelUpdate;
            damageLevelComponent.OnLevelDecrease -= OnDamageLevelUpdate;
        }
    }
}