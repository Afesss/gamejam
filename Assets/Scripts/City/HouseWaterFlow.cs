using UnityEngine;

[RequireComponent(typeof(HouseVitality))]
public class HouseWaterFlow : MonoBehaviour
{
    [Tooltip("Настройки дома")]
    [SerializeField]
    private HouseSettings config;

    [Tooltip("Компонент уровня поврежденности здания")]
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
            EventBroker.OnHouseWaterFlowInvoke(currentDamageLevel * config.WaterFlowAmountByLevel * Time.deltaTime);
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