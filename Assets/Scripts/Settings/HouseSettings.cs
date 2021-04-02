using UnityEngine;

[CreateAssetMenu(fileName = "HouseSettings", menuName = "BeaverGame/House", order = 0)]
public class HouseSettings : ScriptableObject
{
    /// <summary>
    /// ���������� ������
    /// </summary>
    public int LevelCount { get { return levelCount; } }
    /// <summary>
    /// ���������� ���� �� ������ �����
    /// </summary>
    public float WindowOffset { get { return windowOffset; } }
    /// <summary>
    /// ��� �������� � ���������� ������ �����������
    /// </summary>
    public float DamageLevelStep { get { return damageLevelStep; } }
    /// <summary>
    /// ������������ ���������� �����
    /// </summary>
    public float MaxHealthPoint { get { return maxHealthPoint; } }
    /// <summary>
    /// ���������� ����������� ����������� � �������
    /// </summary>
    public float DamagePerSecond { get { return damagePerSecond; } }
    /// <summary>
    /// ���������� ������������ ����� ������� � �������
    /// </summary>
    public float ReparePerSecond { get { return reparePerSecond; } }
    /// <summary>
    /// ���������� ������������ ���� �� ���� ������� �����������
    /// </summary>
    public float WaterFlowAmountByLevel { get { return waterFlowAmountByLevel; } }

    [Header("��������� ���������")]

    [Tooltip("���������� ������")]
    [SerializeField]
    private int levelCount;

    [Tooltip("���������� ���� �� ������ �����")]
    [SerializeField]
    private float windowOffset;

    [Header("������� �����������")]

    [Tooltip("��� �������� � ���������� ������ �����������")]
    [SerializeField]
    private float damageLevelStep;

    [Header("���������������� ����")]

    [Tooltip("������������ ���������� �����")]
    [SerializeField]
    private float maxHealthPoint;

    [Tooltip("���������� ����������� ����������� � �������")]
    [SerializeField]
    private float damagePerSecond;

    [Tooltip("���������� ������������ ����� ������� � �������")]
    [SerializeField]
    private float reparePerSecond;

    [Header("������� �� ���")]

    [Tooltip("���������� ������������ ���� �� ���� ������� �����������")]
    [SerializeField]
    private float waterFlowAmountByLevel;
}
