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

    /// <summary>
    /// Секунд на один тик проверки
    /// </summary>
    public float DetectionTickDuration { get { return detectionTickDuration; } }

    /// <summary>
    /// Шанс обнаружения на человека в доме
    /// </summary>
    public float DetectionRatePerHealthUnit { get { return detectionRatePerHealthUnit; } }

    /// <summary>
    /// Секунд на побег при обнаружении
    /// </summary>
    public float TimeToFlee { get { return timeToFlee; } }

    /// <summary>
    /// Отступ кнопки побега от центральной оси дома
    /// </summary>
    public float FleeButtonOffset { get { return fleeButtonOffset; } }

    public AudioClip BeaverDetectedAudio { get { return beaverDetectedAudio; } }
    public float BeaverDetectedAudioVolume { get { return beaverDetectedAudioVolume; } }
    public AudioClip HouseRecievedDamageAudio { get { return houseRecievedDamageAudio; } }

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

    [Header("Уровень повреждений")]

    [Space]
    [Tooltip("Шаг перехода к следующему уровню повреждения")]
    [SerializeField]
    private float damageLevelStep;

    [Header("Жизнеспособность дома")]

    [Space]
    [Tooltip("Максимальное количество жизни")]
    [SerializeField]
    private float maxHealthPoint;

    [Tooltip("Количество повреждения получаемого в секунду")]
    [SerializeField]
    private float damagePerSecond;

    [Tooltip("Количество генерируемых очков починки в секунду")]
    [SerializeField]
    private float reparePerSecond;

    [Header("Шоколадки")]

    [Space]
    [Tooltip("Максимальное количество шоколада")]
    [SerializeField]
    private int maxChocolateAmount;

    [Tooltip("Минимальное количество шоколада")]
    [SerializeField]
    private int minChocolateAmount;

    [Tooltip("Количество шоколада, которое бобер крадет за секунду")]
    [SerializeField]
    private float chocolateStealPerSecond;

    [Header("Влияние на мир")]

    [Space]
    [Tooltip("Количество генерируемой воды в секунду за один уровень повреждения")]
    [SerializeField]
    private float waterFlowAmountByLevel;

    [Header("Обнаружение бобров")]

    [Space]
    [Tooltip("Секунд на один тик проверки")]
    [SerializeField]
    private float detectionTickDuration = 3;

    [Tooltip("Шанс обнаружения в доме на единицу жизни дома")]
    [SerializeField]
    [Range(0, 0.05f)]
    private float detectionRatePerHealthUnit = 0.005f;

    [Tooltip("Секунд на побег при обнаружении")]
    [SerializeField]
    private float timeToFlee = 3;

    [Tooltip("Отступ кнопки побега от центральной оси дома")]
    [SerializeField]
    private float fleeButtonOffset = 1.2f;

    [SerializeField]
    private AudioClip beaverDetectedAudio;

    [SerializeField]
    [Range(0f,1f)]
    private float beaverDetectedAudioVolume;

    [SerializeField]
    private AudioClip houseRecievedDamageAudio;


}
