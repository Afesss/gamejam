using UnityEngine;

[CreateAssetMenu(fileName = "CitySettings", menuName = "BeaverGame/City", order = 1)]
public class CitySettings : ScriptableObject
{
    /// <summary>
    /// ���������� ������� ��� �����
    /// </summary>
    public int CityColsCount { get { return cityColsCount; } }

    /// <summary>
    /// ���������� ����� ��� �����
    /// </summary>
    public int CityRowsCount { get { return cityRowsCount; } }

    /// <summary>
    /// ������ ������ �� ������� ����� ���
    /// </summary>
    public Vector2 CellSize { get { return cellSize; } }

    /// <summary>
    /// ������� �����
    /// </summary>
    public GameObject[] HouseObjects { get { return houseObjects; } }

    /// <summary>
    /// ���������� ����������� �����
    /// </summary>
    public int[] HouseRates { get { return houseRates; } }

    /// <summary>
    /// ������������ ���������� ����
    /// </summary>
    public float MaxWaterLevel { get { return maxWaterLevel; } }

    /// <summary>
    /// ������������ ������ ������ ���� �� ����� (��� Y)
    /// </summary>
    public float MaxWaterYPosition { get { return maxWaterYPosition; } }

    /// <summary>
    /// �������� �������� ���� (���������� � �������)
    /// </summary>
    public float OutflowSpeedPerSecond { get { return outflowSpeedPerSecond; } }


    [Header("��������� ������")]

    [Tooltip("���������� ������� ��� �����")]
    [SerializeField]
    private int cityColsCount;

    [Tooltip("���������� ����� ��� �����")]
    [SerializeField]
    private int cityRowsCount;

    [Tooltip("������ ������ �� ������� ����� ���")]
    [SerializeField]
    private Vector2 cellSize;

    [Tooltip("������� �����")]
    [SerializeField]
    private GameObject[] houseObjects;

    [Tooltip("���������� ����������� �����")]
    [SerializeField]
    private int[] houseRates;

    [Header("������� ������������� ������")]

    [Tooltip("������������ ���������� ����")]
    [SerializeField]
    private float maxWaterLevel = 3000;

    [Tooltip("������������ ������ ������ ���� �� ����� (��� Y)")]
    [SerializeField]
    private float maxWaterYPosition = 10;

    [Tooltip("�������� �������� ���� (���������� � �������)")]
    [SerializeField]
    private float outflowSpeedPerSecond = 10;


}
