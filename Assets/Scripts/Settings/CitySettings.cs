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
    /// Объекты домов
    /// </summary>
    public GameObject[] HouseObjects { get { return houseObjects; } }

    /// <summary>
    /// Процентное соотношение домов
    /// </summary>
    public int[] HouseRates { get { return houseRates; } }

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

    [Tooltip("Объекты домов")]
    [SerializeField]
    private GameObject[] houseObjects;

    [Tooltip("Процентное соотношение домов")]
    [SerializeField]
    private int[] houseRates;

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


}
