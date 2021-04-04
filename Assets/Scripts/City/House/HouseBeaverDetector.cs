using UnityEngine;
using System;

public class HouseBeaverDetector : MonoBehaviour
{
    public delegate void DetectorAction(Vector3 detectPosition);
    public event DetectorAction OnDetectBeaver;
    public event Action OnDestroyBeaver;

    [Tooltip("Настройки дома")]
    [SerializeField]
    private HouseSettings config;

    [Tooltip("Компонент жизнеспособности дома")]
    [SerializeField]
    private HouseVitality vitality;

    [Tooltip("Компонент шоколад")]
    [SerializeField]
    private HouseChocolate chocolate;

    [Tooltip("Меш дома")]
    [SerializeField]
    private MeshRenderer mesh;

    private float tickCountdown;
    private float fleeCountdown;

    private bool isBeaverDetected = false;
    private int currentMinLevel = 0;

    private AudioSource detectedAudio = null;

    private void Awake()
    {
        tickCountdown = config.DetectionTickDuration;
        fleeCountdown = config.TimeToFlee;

        detectedAudio = gameObject.AddComponent<AudioSource>();
        detectedAudio.clip = config.BeaverDetectedAudio;
        detectedAudio.volume = config.BeaverDetectedAudioVolume;
        detectedAudio.loop = false;

        CityData.Instance.worldWaterLevel.OnFloodLevelChange += OnFloodLevelChange;
    }

    private void OnFloodLevelChange(int level)
    {
        currentMinLevel = level;
    }

    private void Update()
    {
        // если бобер убежал - сбрасываем все состояния
        if ((!vitality.IsRecieveDamage && !chocolate.IsStealingActive) && (isBeaverDetected || tickCountdown < config.DetectionTickDuration))
        {
            tickCountdown = config.DetectionTickDuration;
            fleeCountdown = config.TimeToFlee;
            isBeaverDetected = false;
            detectedAudio.Stop();
        }

        // если бобер обнаружен даем время на побег
        if (isBeaverDetected)
        {
            if (fleeCountdown <= 0)
            {
                OnDestroyBeaver?.Invoke();
                isBeaverDetected = false;
                detectedAudio.Stop();
                fleeCountdown = config.TimeToFlee;
                chocolate.IsStealingActive = false;
                vitality.IsRecieveDamage = false;
            }
            else
            {
                fleeCountdown -= Time.deltaTime;
            }
            return;
        }

        // если завелся бобер - идут попытки его обнаружения
        if ((vitality.IsRecieveDamage || chocolate.IsStealingActive) && !isBeaverDetected)
        {
            if (tickCountdown <= 0 && vitality.HealthPoint > 0)
            {
                var rate = Mathf.Clamp01(vitality.HealthPoint * config.DetectionRatePerHealthUnit);
                
                if (UnityEngine.Random.Range(0f, 1f) < rate)
                {
                    var point = GetRandomPoint();
                    OnDetectBeaver?.Invoke(point);
                    fleeCountdown = config.TimeToFlee;
                    isBeaverDetected = true;
                    detectedAudio.Play();
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
        var randomLevel = UnityEngine.Random.Range(currentMinLevel, config.LevelCount);
        var levelCenter = groundCenter + new Vector3(0, floorHeight * randomLevel + config.WindowFloorOffset, 0);

        var dirs = new Vector3[] {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
        var dir = dirs[UnityEngine.Random.Range(0, dirs.Length)] * mesh.bounds.extents.x * config.FleeButtonOffset;


        return levelCenter + dir;
    }

    private void OnDestroy()
    {
        if (CityData.Instance != null && CityData.Instance.worldWaterLevel != null)
            CityData.Instance.worldWaterLevel.OnFloodLevelChange -= OnFloodLevelChange;
    }
}