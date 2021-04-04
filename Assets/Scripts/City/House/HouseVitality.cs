using System;
using UnityEngine;

/// <summary>
/// Компонент жизнеспособности дома
/// </summary>
[SelectionBaseAttribute]
public class HouseVitality : MonoBehaviour
{
    [Tooltip("Получает ли дом повреждения")]
    [SerializeField]
    private bool isRecieveDamage;

    [Tooltip("Затоплен ли дом")]
    [SerializeField]
    private bool isFlooded;

    [Tooltip("Настройки дома")]
    [SerializeField]
    private HouseSettings config;

    [SerializeField]
    private HouseController houseController;

    internal bool IsFlooded { get { return isFlooded; } }
    /// <summary>
    /// Текущее количество жизни у дома
    /// </summary>
    public float HealthPoint { get { return healthPoint; } }

    /// <summary>
    /// Получает ли дом повреждения
    /// </summary>
    public bool IsRecieveDamage { get { return isRecieveDamage; } set { isRecieveDamage = value; } }

    /// <summary>
    /// Доля жизни дома
    /// </summary>
    public float HealthPointRate { get { return Mathf.Clamp01(healthPoint / config.MaxHealthPoint); } }

    private float healthPoint;
    private float maxHealthPoint;

    private void Awake()
    {
        healthPoint = config.MaxHealthPoint;
        maxHealthPoint = config.MaxHealthPoint;
        CityData.Instance.worldWaterLevel.OnFloodLevelChange += OnFloodLevelChange;
    }

    private void OnFloodLevelChange(int level)
    {
        maxHealthPoint = config.MaxHealthPoint - level * config.DamageLevelStep * config.MaxHealthPoint;
        if (healthPoint > maxHealthPoint)
            healthPoint = maxHealthPoint;
    }

    private void Update()
    {
        if (isRecieveDamage && healthPoint > 0)
            DecreaseHealthTick();
        else if (!isRecieveDamage && !isFlooded && healthPoint < maxHealthPoint)
            IncreaseHealthTick();

        if (isFlooded && healthPoint > 0)
            healthPoint = 0;
    }

    private void DecreaseHealthTick()
    {
        healthPoint -= config.DamagePerSecond * Time.deltaTime;
        if (healthPoint <= 0)
        {
            houseController._animation.Stop();
            houseController.HomeButton.SetActive(true);
            //TODO: раскоментировать если необходимо чтобы после полной поломки дом не ремонтировался
            //isFlooded = true;
            //TODO: раскоментировать если необходимо отключить состояние повреждения после полной поломки
            //isRecieveDamage = false;
        }
    }

    private void IncreaseHealthTick()
    {
        healthPoint += config.ReparePerSecond * Time.deltaTime;
    }

    private void OnDestroy()
    {
        if (CityData.Instance != null && CityData.Instance.worldWaterLevel != null)
            CityData.Instance.worldWaterLevel.OnFloodLevelChange -= OnFloodLevelChange;
    }
}