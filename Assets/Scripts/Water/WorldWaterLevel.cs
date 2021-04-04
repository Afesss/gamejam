using System;
using UnityEngine;

public class WorldWaterLevel : MonoBehaviour
{
    public delegate void FloodLevelAction(int level);
    public event FloodLevelAction OnFloodLevelChange;

    public float WaterLevelRate { get { return currentWaterLevel / config.MaxWaterLevel; } }
    public int FloodLevel { get { return currentFloodLevel; } }

    [Tooltip("Настройки города")]
    [SerializeField]
    private CitySettings config;

    private float currentWaterLevel = 0;

    private int maxHouseLevel;
    private float waterAmountToLevel = 0;

    private int currentFloodLevel = 0;
    private Transform _transform;
    private AudioSource waterAudio = null;

    private void Awake()
    {
        EventBroker.OnHouseWaterFlow += OnHouseWaterFlow;
        maxHouseLevel = config.HouseSets.Length;
        waterAmountToLevel = config.MaxWaterLevel / maxHouseLevel;
        _transform = transform;

        waterAudio = gameObject.AddComponent<AudioSource>();
        waterAudio.clip = config.WaterLevelClip;
        waterAudio.volume = 0;
        waterAudio.loop = true;
    }

    private void Start()
    {
        waterAudio.Play();
    }

    private void Update()
    {
        EventBroker.SendWaterPositionInvoke(_transform.position.y);
        if (currentWaterLevel < config.MaxWaterLevel)
        {
            var yPos = currentWaterLevel / config.MaxWaterLevel * config.MaxWaterYPosition;
            var curPos = transform.position;
            transform.position = new Vector3(curPos.x, yPos, curPos.z);
        }

        var limit = currentFloodLevel * waterAmountToLevel;
        if (currentWaterLevel > limit)
            currentWaterLevel = Mathf.Clamp(currentWaterLevel - config.OutflowSpeedPerSecond * Time.deltaTime, limit, float.MaxValue);

        var newLevel = Mathf.FloorToInt(currentWaterLevel / waterAmountToLevel);
        if (newLevel != currentFloodLevel)
        {
            OnFloodLevelChange?.Invoke(newLevel);
            currentFloodLevel = newLevel;
            if (newLevel >= maxHouseLevel)
            {
                waterAudio.Stop();
                EventBroker.OnFloodingCompleteInvoke();
            }
        }
    }

    private void OnHouseWaterFlow(float amount)
    {
        if (currentWaterLevel < config.MaxWaterLevel)
            currentWaterLevel += amount;

        UpdateSoundVolume();
    }

    private void UpdateSoundVolume()
    {
        waterAudio.volume = config.MinWaterLevelClipVolume + WaterLevelRate * (config.MaxWaterLevelClipVolume - config.MinWaterLevelClipVolume);
    }

    private void OnDestroy()
    {
        EventBroker.OnHouseWaterFlow -= OnHouseWaterFlow;
    }
}