using System;
using UnityEngine;

public class WorldWaterLevel : MonoBehaviour
{
    public delegate void FloodLevelAction(int level);
    public event FloodLevelAction OnFloodLevelChange;
    public event Action OnFloodingComplete;

    public float WaterLevelRate { get { return currentWaterLevel / config.MaxWaterLevel; } }
    public int FloodLevel { get { return currentFloodLevel; } }

    [Tooltip("Настройки города")]
    [SerializeField]
    private CitySettings config;

    private float currentWaterLevel = 0;

    private int maxHouseLevel;
    private float waterAmountToLevel = 0;

    private int currentFloodLevel = 0;

    private void Awake()
    {
        EventBroker.OnHouseWaterFlow += OnHouseWaterFlow;
        maxHouseLevel = config.HouseSets.Length;
        waterAmountToLevel = config.MaxWaterLevel / maxHouseLevel;
    }

    private void Update()
    {
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
        }
    }

    private void OnHouseWaterFlow(float amount)
    {
        if (currentWaterLevel < config.MaxWaterLevel)
            currentWaterLevel += amount;
        
        if (currentWaterLevel >= config.MaxWaterLevel)
            OnFloodingComplete?.Invoke();
    }

    private void OnDestroy()
    {
        EventBroker.OnHouseWaterFlow -= OnHouseWaterFlow;
    }
}