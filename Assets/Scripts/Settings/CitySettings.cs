using UnityEngine;

[CreateAssetMenu(fileName = "CitySettings", menuName = "BeaverGame/City", order = 1)]
public class CitySettings : ScriptableObject
{
    /// <summary>
    /// Количество колонок для домов
    /// </summary>
    public int CityColsCount { get { return cityColsCount; } }

    /// <summary>
    /// Количество линий для домов
    /// </summary>
    public int CityRowsCount { get { return cityRowsCount; } }

    /// <summary>
    /// Размер ячейки на которой стоит дом
    /// </summary>
    public Vector2 CellSize { get { return cellSize; } }

    /// <summary>
    /// Набор домов
    /// </summary>
    public House[] HouseSets { get { return houseSets; } }

    /// <summary>
    /// Наборы материалов
    /// </summary>
    public MaterialSet[] MaterialSets { get { return materialSets; } }

    /// <summary>
    /// Объекты крыш
    /// </summary>
    public GameObject[] RoofObjects { get { return roofObjects; } }

    /// <summary>
    /// Максимальное количество воды
    /// </summary>
    public float MaxWaterLevel { get { return maxWaterLevel; } }

    /// <summary>
    /// Максимальная высота уровня воды на сцене (ось Y)
    /// </summary>
    public float MaxWaterYPosition { get { return maxWaterYPosition; } }

    /// <summary>
    /// Скорость убывания воды (количество в секунду)
    /// </summary>
    public float OutflowSpeedPerSecond { get { return outflowSpeedPerSecond; } }

    public AudioClip WaterLevelClip { get { return waterLevelClip; } }

    public float MinWaterLevelClipVolume { get { return minWaterLevelClipVolume; } }

    public float MaxWaterLevelClipVolume { get { return maxWaterLevelClipVolume; } }

    [Header("Генератор города")]

    [Tooltip("Количество колонок для домов")]
    [SerializeField]
    private int cityColsCount;

    [Tooltip("Количество линий для домов")]
    [SerializeField]
    private int cityRowsCount;

    [Tooltip("Размер ячейки на которой стоит дом")]
    [SerializeField]
    private Vector2 cellSize;

    [Tooltip("Объекты домов (разместить в порядке количества этажей)")]
    [SerializeField]
    private House[] houseSets;

    [Tooltip("Наборы материалов (Применяются в порядке размещения)")]
    [SerializeField]
    private MaterialSet[] materialSets;

    [Tooltip("Объекты крыш")]
    [SerializeField]
    private GameObject[] roofObjects;

    [Space]
    [Header("Уровень затопленности города")]

    [Tooltip("Максимальное количество воды")]
    [SerializeField]
    private float maxWaterLevel = 3000;

    [Tooltip("Максимальная высота уровня воды на сцене (ось Y)")]
    [SerializeField]
    private float maxWaterYPosition = 10;

    [Tooltip("Скорость убывания воды (количество в секунду)")]
    [SerializeField]
    private float outflowSpeedPerSecond = 10;

    [SerializeField]
    private AudioClip waterLevelClip;

    [SerializeField]
    [Range(0f, 1f)]
    private float minWaterLevelClipVolume = 0;

    [SerializeField]
    [Range(0f, 1f)]
    private float maxWaterLevelClipVolume = 1;
}
