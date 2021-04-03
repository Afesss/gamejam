using UnityEngine;
using System;

public class HouseBeaverDetector : MonoBehaviour
{
    public delegate void DetectorAction(Vector3 detectPosition);
    public static event DetectorAction OnDetectBeaver;
    public static event Action OnDestroyBeaver;

    [Tooltip("Настройки дома")]
    [SerializeField]
    private HouseSettings config;

    [Tooltip("Компонент жизнеспособности дома")]
    [SerializeField]
    private HouseVitality vitality;

    [Tooltip("Меш дома")]
    [SerializeField]
    private MeshRenderer mesh;

    private float tickCountdown;
    private float fleeCountdown;

    private bool isBeaverDetected = false;

    // TODO: Удалить после отладки
    private Vector3 tmpDetectedPoint = Vector3.zero;

    private void Awake()
    {
        tickCountdown = config.DetectionTickDuration;
        fleeCountdown = config.TimeToFlee;

        //GetRandomPoint();
    }

    private void Update()
    {
        // если бобер убежал - сбрасываем все состояния
        if (!vitality.IsRecieveDamage && (isBeaverDetected || tickCountdown < config.DetectionTickDuration))
        {
            tickCountdown = config.DetectionTickDuration;
            fleeCountdown = config.TimeToFlee;
            isBeaverDetected = false;
        }

        // если бобер обнаружен даем время на побег
        if (isBeaverDetected)
        {
            if (fleeCountdown <= 0)
            {
                Debug.Log("You DIE!");
                OnDestroyBeaver?.Invoke();
                isBeaverDetected = false;
                fleeCountdown = config.TimeToFlee;
                vitality.IsRecieveDamage = false;

                tmpDetectedPoint = Vector3.zero;
            }
            else
            {
                fleeCountdown -= Time.deltaTime;
            }
            return;
        }

        // если завелся бобер - идут попытки его обнаружения
        if (vitality.IsRecieveDamage && !isBeaverDetected)
        {
            if (tickCountdown <= 0 && vitality.HealthPoint > 0)
            {
                var rate = Mathf.Clamp01(vitality.HealthPoint * config.DetectionRatePerHealthUnit);
                
                if (UnityEngine.Random.Range(0f, 1f) < rate)
                {
                    Debug.Log("GOCHA!");
                    var point = GetRandomPoint();
                    OnDetectBeaver?.Invoke(point);
                    isBeaverDetected = true;
                }

                tickCountdown = config.DetectionTickDuration;
            }

            // ведем отсчет 
            if (tickCountdown > 0)
            {
                tickCountdown -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Возвращает случайную точку с одной из сторон дома на случайном этаже
    /// </summary>
    /// <returns>Случайная точка</returns>
    private Vector3 GetRandomPoint()
    {
        var floorHeight = mesh.bounds.size.y / config.LevelCount - config.RoofHeight;
        var groundCenter = new Vector3(mesh.bounds.center.x, 0, mesh.bounds.center.z);
        var randomLevel = UnityEngine.Random.Range(0, config.LevelCount);
        var levelCenter = groundCenter + new Vector3(0, floorHeight * randomLevel + config.WindowFloorOffset, 0);

        var dirs = new Vector3[] {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
        var dir = dirs[UnityEngine.Random.Range(0, dirs.Length)] * mesh.bounds.extents.x * config.FleeButtonOffset;

        tmpDetectedPoint = levelCenter + dir;

        return levelCenter + dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (tmpDetectedPoint != Vector3.zero)
            Gizmos.DrawSphere(tmpDetectedPoint, .5f);
        Gizmos.color = Color.white;
    }
}