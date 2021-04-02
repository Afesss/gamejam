using UnityEngine;

public class WorldWaterLevel : MonoBehaviour
{
    [Tooltip("Настройки города")]
    [SerializeField]
    private CitySettings config;

    public float WaterLevelRate { get { return currentWaterLevel / config.MaxWaterLevel; } }

    private float currentWaterLevel = 0;

    private void Awake()
    {
        EventBroker.OnHouseWaterFlow += OnHouseWaterFlow;
    }

    private void Update()
    {
        if (currentWaterLevel < config.MaxWaterLevel)
        {
            var yPos = currentWaterLevel / config.MaxWaterLevel * config.MaxWaterYPosition;
            var curPos = transform.position;
            transform.position = new Vector3(curPos.x, yPos, curPos.z);
        }

        if (currentWaterLevel > 0)
            currentWaterLevel -= config.OutflowSpeedPerSecond * Time.deltaTime;
    }

    private void OnHouseWaterFlow(float amount)
    {
        if (currentWaterLevel < config.MaxWaterLevel)
            currentWaterLevel += amount;
    }

    private void OnDestroy()
    {
        EventBroker.OnHouseWaterFlow -= OnHouseWaterFlow;
    }
}