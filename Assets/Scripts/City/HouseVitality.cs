using UnityEngine;

/// <summary>
/// Компонент жизнеспособности дома
/// </summary>
public class HouseVitality : MonoBehaviour
{
    [Tooltip("Получает ли дом повреждения")]
    [SerializeField]
    private bool isRecieveDamage;

    [Tooltip("Затоплен ли дом")]
    [SerializeField]
    private bool isFlooded;

    [Tooltip("Максимальное количество жизни")]
    [SerializeField]
    private float maxHealthPoint;
    

    [Tooltip("Количество повреждения получаемого в секунду")]
    [SerializeField]
    private float damagePerSecond;

    [Tooltip("Количество генерируемых очков починки в секунду")]
    [SerializeField]
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
        healthPoint = maxHealthPoint;
    }

    private void Update()
    {
        if (isRecieveDamage && healthPoint > 0)
            healthPoint -= damagePerSecond * Time.deltaTime;
        else if (!isRecieveDamage && !isFlooded && healthPoint < maxHealthPoint)
            healthPoint += reparePerSecond * Time.deltaTime;

        if (isFlooded && healthPoint > 0)
            healthPoint = 0;
    }
}