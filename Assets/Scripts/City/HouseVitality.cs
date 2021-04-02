using UnityEngine;

/// <summary>
/// ��������� ���������������� ����
/// </summary>
[SelectionBaseAttribute]
public class HouseVitality : MonoBehaviour
{
    [Tooltip("�������� �� ��� �����������")]
    [SerializeField]
    private bool isRecieveDamage;

    [Tooltip("�������� �� ���")]
    [SerializeField]
    private bool isFlooded;

    [Tooltip("��������� ����")]
    [SerializeField]
    private HouseSettings config;

    // TODO: ������ ��� ���� � ����� ��� ��� ������������ - ������������ ����� config
    /// <summary>
    /// ������������ ���������� �����
    /// </summary>
    private float maxHealthPoint;
    /// <summary>
    /// ���������� ����������� ����������� � �������
    /// </summary>
    private float damagePerSecond;
    /// <summary>
    /// ���������� ������������ ����� ������� � �������
    /// </summary>
    private float reparePerSecond;

    /// <summary>
    /// ������� ���������� ����� � ����
    /// </summary>
    public float HealthPoint { get { return healthPoint; } }
    public bool IsRecieveDamage { get { return isRecieveDamage; } set { isRecieveDamage = value; } }

    /// <summary>
    /// ���� ����� ����
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