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

        CityData.Instance.worldWaterLevel.OnFloodLevelChange += OnFloodLevelChange;
    }

    private void OnFloodLevelChange(int level)
    {
        StopDamageLevelAnimation(level - 1);
    }

    private void Start()
    {
        floorHeight = mesh.bounds.size.y / config.LevelCount - config.RoofHeight;
        groundCenter = new Vector3(mesh.bounds.center.x, 0, mesh.bounds.center.z);
        windowPosition = new Vector3[config.LevelCount][];
        windowWater = new WaterfallPoolObject[config.LevelCount][];

        GenerateWindowPosition();
    }

    private void OnDamageLevelIncrease(int level)
    {
        StartDamageLevelAnimation(level - 1);
    }

    private void OnDamageLevelDecrease(int level)
    {
        StopDamageLevelAnimation(level);
    }

    private void StartDamageLevelAnimation(int levelIndex)
    {
        var levelWindowPositions =  windowPosition[levelIndex];
        windowWater[levelIndex] = new WaterfallPoolObject[levelWindowPositions.Length];
        var startAngle = 0;
        for (var i = 0; i < levelWindowPositions.Length; i++)
        {
            if (!float.IsInfinity(levelWindowPositions[i].x))
            {
                var angle = startAngle + (i / 2) * 90;
                windowWater[levelIndex][i] = CityData.Instance.waterfallPoolService.GetFreeElement();
                windowWater[levelIndex][i].transform.rotation = Quaternion.Euler(0, angle, 0);
                windowWater[levelIndex][i].transform.position = levelWindowPositions[i];
            }
        }
    }

    private void StopDamageLevelAnimation(int levelIndex)
    {
        if (levelIndex < config.LevelCount && windowWater[levelIndex] != null)
        {
            var levelWindowPositions = windowPosition[levelIndex];
            for (var i = 0; i < levelWindowPositions.Length; i++)
                if (windowWater[levelIndex][i] != null)
                    windowWater[levelIndex][i].ReturnToPool();
        }
    }

    private void GenerateWindowPosition()
    {
        var windowCount = 8;
        var sizeX = mesh.bounds.size.x;
        var sizeZ = mesh.bounds.size.z;
        for (var i = 0; i < config.LevelCount; i++)
        {
            var centerLevel = groundCenter + Vector3.up * floorHeight * i;

            var yPosition = centerLevel.y + config.WindowFloorOffset;

            var zBoundForward = centerLevel.z + mesh.bounds.extents.z - config.WindowMeshOffset;
            var zBoundBackward = centerLevel.z - mesh.bounds.extents.z + config.WindowMeshOffset;
            var xBoundRight = centerLevel.x + mesh.bounds.extents.x - config.WindowMeshOffset;
            var xBoundLeft = centerLevel.x - mesh.bounds.extents.x + config.WindowMeshOffset;

            windowPosition[i] = new Vector3[windowCount];
            windowPosition[i][0] = new Vector3(centerLevel.x - sizeX / 5, yPosition, zBoundForward);
            windowPosition[i][1] = new Vector3(centerLevel.x + sizeX / 5, yPosition, zBoundForward);
            windowPosition[i][2] = new Vector3(xBoundRight, yPosition, centerLevel.z + sizeZ / 5);
            windowPosition[i][3] = new Vector3(xBoundRight, yPosition, centerLevel.z - sizeZ / 5);
            // не показываем воду на первом этаже там где дверь
            windowPosition[i][4] = (i != 0)? new Vector3(centerLevel.x + sizeX / 5, yPosition, zBoundBackward): Vector3.negativeInfinity;
            windowPosition[i][5] = (i != 0)? new Vector3(centerLevel.x - sizeX / 5, yPosition, zBoundBackward): Vector3.negativeInfinity;
            windowPosition[i][6] = new Vector3(xBoundLeft, yPosition, centerLevel.z - sizeZ / 5);
            windowPosition[i][7] = new Vector3(xBoundLeft, yPosition, centerLevel.z + sizeZ / 5);
        }
    }

    private void OnDestroy()
    {
        if (damageLevelComponent != null)
        {
            damageLevelComponent.OnLevelIncrease -= OnDamageLevelIncrease;
            damageLevelComponent.OnLevelDecrease -= OnDamageLevelDecrease;
        }
        if (CityData.Instance != null && CityData.Instance.worldWaterLevel != null)
            CityData.Instance.worldWaterLevel.OnFloodLevelChange -= OnFloodLevelChange;
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
                    if (j == 0)
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;
                    Gizmos.DrawSphere(windowPosition[i][j], 0.1f);
                    Gizmos.color = Color.white;
                }
            }
        }
    }
}
