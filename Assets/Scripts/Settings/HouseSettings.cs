using UnityEngine;

[CreateAssetMenu(fileName = "HouseSettings", menuName = "BeaverGame/House", order = 0)]
public class HouseSettings : ScriptableObject
{
    /// <summary>
    /// Количество этажей
    /// </summary>
    public int LevelCount { get { return levelCount; } }
    /// <summary>
    /// Высота крыши
    /// </summary>
    public float RoofHeight { get { return roofHeight; } }
    /// <summary>
    /// Расстояние окна от начала этажа
    /// </summary>
    public float WindowFloorOffset { get { return windowFloorOffset; } }
    /// <summary>
    /// Расстояние между окном и границей меша
    /// </summary>
    public float WindowMeshOffset { get { return windowMeshOffset; } }
    /// <summary>
    /// Шаг перехода к следующему уровню повреждения
    /// </summary>
    public float DamageLevelStep { get { return damageLevelStep; } }
    /// <summary>
    /// Максимальное количество жизни
    /// </summary>
    public float MaxHealthPoint { get { return maxHealthPoint; } }
    /// <summary>
    /// Количество повреждения получаемого в секунду
    /// </summary>
    public float DamagePerSecond { get { return damagePerSecond; } }
    /// <summary>
    /// Количество генерируемых очков починки в секунду
    /// </summary>
    public float ReparePerSecond { get { return reparePerSecond; } }
    /// <summary>
    /// Максимальное количество шоколада
    /// </summary>
    public int MaxChocolateAmount { get { return maxChocolateAmount; } }
    /// <summary>
    /// Минимальное количество шоколада
    /// </summary>
    public int MinChocolateAmount { get { return minChocolateAmount; } }
    /// <summary>
    /// Количество шоколада, которое бобер крадет за секунду
    /// </summary>
    public float СhocolateStealPerSecond { get { return chocolateStealPerSecond; } }

    /// <summary>
    /// Количество генерируемой воды в секунду за один уровень повреждения
    /// </summary>
    public float WaterFlowAmountByLevel { get { return waterFlowAmountByLevel; } }

    [Header("Настройка геометрии")]

    [Tooltip("Количество этажей")]
    [SerializeField]
    private int levelCount;

    [Tooltip("Высота крыши")]
    [SerializeField]
    private float roofHeight;

    [Tooltip("Расстояние окна от начала этажа")]
    [SerializeField]
    private float windowFloorOffset;

    [Tooltip("Расстояние между окном и границей меша")]
    [SerializeField]
    private float windowMeshOffset;

    [Tooltip("Есть ли у дома крыша")]
    [SerializeField]
    private bool hasRoof;

    [Space]
    [Header("Уровень повреждений")]

    [Tooltip("Шаг перехода к следующему уровню повреждения")]
    [SerializeField]
    private float damageLevelStep;

    [Space]
    [Header("Жизнеспособность дома")]

    [Tooltip("Максимальное количество жизни")]
    [SerializeField]
    private float maxHealthPoint;

    [Tooltip("Количество повреждения получаемого в секунду")]
    [SerializeField]
    private float damagePerSecond;

    [Tooltip("Количество генерируемых очков починки в секунду")]
    [SerializeField]
    private float reparePerSecond;

    [Space]
    [Header("Жизнеспособность дома")]

    [Tooltip("Максимальное количество шоколада")]
    [SerializeField]
    private int maxChocolateAmount;

    [Tooltip("Минимальное количество шоколада")]
    [SerializeField]
    private int minChocolateAmount;

    [Tooltip("Количество шоколада, которое бобер крадет за секунду")]
    [SerializeField]
    private float chocolateStealPerSecond;

    [Space]
    [Header("Влияние на мир")]

    [Tooltip("Количество генерируемой воды в секунду за один уровень повреждения")]
    [SerializeField]
    private float waterFlowAmountByLevel;
}
