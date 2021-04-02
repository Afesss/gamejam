using UnityEngine;

/// <summary>
/// ��������� ���������������� ����
/// </summary>
public class HouseVitality : MonoBehaviour
{
    [Tooltip("�������� �� ��� �����������")]
    [SerializeField]
    private bool isRecieveDamage;

    [Tooltip("�������� �� ���")]
    [SerializeField]
    private bool isFlooded;

    [Tooltip("������������ ���������� �����")]
    [SerializeField]
    private float maxHealthPoint;
    

    [Tooltip("���������� ����������� ����������� � �������")]
    [SerializeField]
    private float damagePerSecond;

    [Tooltip("���������� ������������ ����� ������� � �������")]
    [SerializeField]
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