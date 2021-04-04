using UnityEngine;

/// <summary>
/// Компонент уровня поврежденности здания
/// </summary>
public class HouseDamageLevel : MonoBehaviour
{
    [Tooltip("Настройки дома")]
    [SerializeField]
    private HouseSettings config;

    [Tooltip("Компонент жизнеспособности дома")]
    [SerializeField]
    private HouseVitality vitality;

    private int damageLevel = 0;
    private int minDamageLevel = 0;

    public delegate void ChangeDamageLevelAction(int level);
    public event ChangeDamageLevelAction OnLevelIncrease;
    public event ChangeDamageLevelAction OnLevelDecrease;

    private void Awake()
    {
        CityData.Instance.worldWaterLevel.OnFloodLevelChange += OnFloodLevelChange;
    }

    private void OnFloodLevelChange(int level)
    {
        minDamageLevel = level;
        if (damageLevel < minDamageLevel)
            damageLevel = minDamageLevel;
    }

    void Update()
    {
        var damageRate = 1 - vitality.HealthPointRate;
        if (damageRate > (damageLevel + 1) * config.DamageLevelStep && damageLevel < config.LevelCount)
            SetNextDamageLevel();
        else if (damageRate < (damageLevel * config.DamageLevelStep) && damageLevel > minDamageLevel)
            SetPreviousDamageLevel();
    }

    private void SetNextDamageLevel()
    {
        damageLevel++;
        OnLevelIncrease?.Invoke(damageLevel);
    }

    private void SetPreviousDamageLevel()
    {
        damageLevel--;
        OnLevelDecrease?.Invoke(damageLevel);
    }

    private void OnDestroy()
    {
        if (CityData.Instance != null && CityData.Instance.worldWaterLevel != null)
            CityData.Instance.worldWaterLevel.OnFloodLevelChange -= OnFloodLevelChange;
    }
}
