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

    // TODO: Убрать эти поля и везде где они используются - использовать сразу config
    /// <summary>
    /// Максимальное количество жизни
    /// </summary>
    private float maxHealthPoint;
    /// <summary>
    /// Количество повреждения получаемого в секунду
    /// </summary>
    private float damagePerSecond;
    /// <summary>
    /// Количество генерируемых очков починки в секунду
    /// </summary>
    private float reparePerSecond;

    /// <summary>
    /// Текущее количество жизни у дома
    /// </summary>
    public float HealthPoint { get { return healthPoint; } }
    public bool IsRecieveDamage { get { return isRecieveDamage; } set { isRecieveDamage = value; } }

    /// <summary>
    /// Доля жизни дома
    /// </summary>
    public float HealthPointRate { get { return Mathf.Clamp01(healthPoint / maxHealthPoint); } }

    private float healthPoint;

    private void Awake()
    {
        maxHealthPoint = config.MaxHealthPoint;
        damagePerSecond = config.DamagePerSecond;
        reparePerSecond = config.ReparePerSecond;
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
        healthPoint -= damagePerSecond * Time.deltaTime;
    }

    private void IncreaseHealthTick()
    {
        healthPoint += reparePerSecond * Time.deltaTime;
    }
}