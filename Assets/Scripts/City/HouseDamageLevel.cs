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

    public delegate void ChangeDamageLevelAction(int level);
    public event ChangeDamageLevelAction OnLevelIncrease;
    public event ChangeDamageLevelAction OnLevelDecrease;

    void Update()
    {
        var damageRate = 1 - vitality.HealthPointRate;
        if (damageRate > (damageLevel + 1) * config.DamageLevelStep && damageLevel < config.LevelCount)
            SetNextDamageLevel();
        else if (damageRate < (damageLevel * config.DamageLevelStep) && damageLevel > 0)
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

}
