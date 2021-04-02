using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HouseVitality))]
public class HouseStateView : MonoBehaviour
{
    [Tooltip("Настройки дома")]
    [SerializeField]
    private HouseSettings config;

    [Tooltip("Меш дома")]
    [SerializeField]
    private MeshRenderer mesh;

    [Tooltip("Компонент уровня поврежденности здания")]
    [SerializeField]
    private HouseDamageLevel damageLevelComponent;

    private float floorHeight;
    private Vector3 groundCenter;
    private Vector3[][] windowPosition;
    private WaterfallPoolObject[][] windowWater;

    private void Awake()
    {
        damageLevelComponent.OnLevelIncrease += OnDamageLevelIncrease;
        damageLevelComponent.OnLevelDecrease += OnDamageLevelDecrease;
    }

    private void Start()
    {
        floorHeight = mesh.bounds.size.y / config.LevelCount;
        groundCenter = new Vector3(mesh.bounds.center.x, 0, mesh.bounds.center.z);
        windowPosition = new Vector3[config.LevelCount][];
        windowWater = new WaterfallPoolObject[config.LevelCount][];

        GenerateWindowPosition();
    }

    private void OnDamageLevelIncrease(int level)
    {
        SetNextDamageLevel(level - 1);
    }

    private void OnDamageLevelDecrease(int level)
    {
        SetPreviousDamageLevel(level);
    }

    private void SetNextDamageLevel(int levelIndex)
    {
        var levelWindowPositions =  windowPosition[levelIndex];
        windowWater[levelIndex] = new WaterfallPoolObject[levelWindowPositions.Length];
        var startAngle = 0;
        for (var i = 0; i < levelWindowPositions.Length; i++)
        {
            var angle = startAngle + (i / 2) * 90;
            windowWater[levelIndex][i] = CityData.Instance.waterfallPoolService.GetFreeElement();
            windowWater[levelIndex][i].transform.rotation = Quaternion.Euler(0, angle, 0);
            windowWater[levelIndex][i].transform.position = levelWindowPositions[i];
        }
    }

    private void SetPreviousDamageLevel(int levelIndex)
    {
        var levelWindowPositions = windowPosition[levelIndex];
        for (var i = 0; i < levelWindowPositions.Length; i++)
            if (windowWater[levelIndex][i] != default)
                windowWater[levelIndex][i].ReturnToPool();
    }

    private void GenerateWindowPosition()
    {
        var windowCount = 8;
        var sizeX = mesh.bounds.size.x;
        var sizeZ = mesh.bounds.size.z;
        for (var i = 0; i < config.LevelCount; i++)
        {
            var centerLevel = groundCenter + Vector3.up * floorHeight * i;

            windowPosition[i] = new Vector3[windowCount];
            windowPosition[i][0] = new Vector3(centerLevel.x - sizeX / 4, centerLevel.y + config.WindowOffset, centerLevel.z + mesh.bounds.extents.z);
            windowPosition[i][1] = new Vector3(centerLevel.x + sizeX / 4, centerLevel.y + config.WindowOffset, centerLevel.z + mesh.bounds.extents.z);
            windowPosition[i][2] = new Vector3(centerLevel.x + mesh.bounds.extents.x, centerLevel.y + config.WindowOffset, centerLevel.z + sizeZ / 4);
            windowPosition[i][3] = new Vector3(centerLevel.x + mesh.bounds.extents.x, centerLevel.y + config.WindowOffset, centerLevel.z - sizeZ / 4);
            windowPosition[i][4] = new Vector3(centerLevel.x + sizeX / 4, centerLevel.y + config.WindowOffset, centerLevel.z - mesh.bounds.extents.z);
            windowPosition[i][5] = new Vector3(centerLevel.x - sizeX / 4, centerLevel.y + config.WindowOffset, centerLevel.z - mesh.bounds.extents.z);
            windowPosition[i][6] = new Vector3(centerLevel.x - mesh.bounds.extents.x, centerLevel.y + config.WindowOffset, centerLevel.z - sizeZ / 4);
            windowPosition[i][7] = new Vector3(centerLevel.x - mesh.bounds.extents.x, centerLevel.y + config.WindowOffset, centerLevel.z + sizeZ / 4);
        }
    }

    private void OnDestroy()
    {
        if (damageLevelComponent != null)
        {
            damageLevelComponent.OnLevelIncrease -= OnDamageLevelIncrease;
            damageLevelComponent.OnLevelDecrease -= OnDamageLevelDecrease;
        }
    }

    private void OnDrawGizmos()
    {
        for(var i = 0; i < config.LevelCount; i++)
        {
            var center = groundCenter + Vector3.up * floorHeight * i;
            Gizmos.DrawWireCube(center, new Vector3(3, 0.01f, 3));

            if (windowPosition != null && windowPosition[i] != null)
            {
                for (var j = 0; j < windowPosition[i].Length; j++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(windowPosition[i][j], 0.1f);
                    Gizmos.color = Color.white;
                }
            }
        }
    }
}
