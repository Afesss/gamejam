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
    private int disabledLevelCount = 0;

    private void Awake()
    {
        damageLevelComponent.OnLevelIncrease += OnDamageLevelUpdate;
        damageLevelComponent.OnLevelDecrease += OnDamageLevelUpdate;
        CityData.Instance.worldWaterLevel.OnFloodLevelChange += OnFloodLevelChange;
    }

    private void OnFloodLevelChange(int level)
    {
        disabledLevelCount = level;
    }

    private void Update()
    {
        var damageLevel = currentDamageLevel - disabledLevelCount;
        if (damageLevel > 0)
            EventBroker.OnHouseWaterFlowInvoke(damageLevel * config.WaterFlowAmountByLevel * Time.deltaTime);
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
        if (CityData.Instance != null && CityData.Instance.worldWaterLevel != null)
            CityData.Instance.worldWaterLevel.OnFloodLevelChange -= OnFloodLevelChange;
    }
}