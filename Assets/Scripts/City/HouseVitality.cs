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

    /// <summary>
    /// Текущее количество жизни у дома
    /// </summary>
    public float HealthPoint { get { return healthPoint; } }
    public bool IsRecieveDamage { get { return isRecieveDamage; } set { isRecieveDamage = value; } }

    /// <summary>
    /// Доля жизни дома
    /// </summary>
    public float HealthPointRate { get { return Mathf.Clamp01(healthPoint / config.MaxHealthPoint); } }

    private float healthPoint;

    private void Awake()
    {
        healthPoint = config.MaxHealthPoint;
    }

    private void Update()
    {
        if (isRecieveDamage && healthPoint > 0)
            DecreaseHealthTick();
        else if (!isRecieveDamage && !isFlooded && healthPoint < config.MaxHealthPoint)
            IncreaseHealthTick();

        if (isFlooded && healthPoint > 0)
            healthPoint = 0;
    }

    private void DecreaseHealthTick()
    {
        healthPoint -= config.DamagePerSecond * Time.deltaTime;
    }

    private void IncreaseHealthTick()
    {
        healthPoint += config.ReparePerSecond * Time.deltaTime;
    }
}