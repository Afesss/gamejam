using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HouseVitality))]
public class HouseStateView : MonoBehaviour
{
    [Tooltip("Количество этажей")]
    [SerializeField]
    private int levelCount;

    [Tooltip("Расстояние окна от начала этажа")]
    [SerializeField]
    private float windowOffset;

    [Tooltip("Меш дома")]
    [SerializeField]
    private MeshRenderer mesh;

    [Tooltip("Компонент жизнеспособности дома")]
    [SerializeField]
    private HouseVitality vitality;

    [Tooltip("Шаг перехода к следующему уровню повреждения")]
    [SerializeField]
    private float damageLevelStep;

    private float floorHeight;
    private Vector3 groundCenter;
    private Vector3[][] windowPosition;
    private WaterfallPoolObject[][] windowWater;

    private int damageLevel = 0;

    private void Start()
    {
        floorHeight = mesh.bounds.size.y / levelCount;
        groundCenter = new Vector3(mesh.bounds.center.x, 0, mesh.bounds.center.z);
        windowPosition = new Vector3[levelCount][];
        windowWater = new WaterfallPoolObject[levelCount][];

        GenerateWindowPosition();
    }

    private void Update()
    {
        var damageRate = 1 - vitality.HealthPointRate;
        if (damageRate > (damageLevel + 1) * damageLevelStep && damageLevel < levelCount)
            SetNextDamageLevel();
        else if (damageRate < (damageLevel * damageLevelStep) && damageLevel > 0)
            SetPreviousDamageLevel();
    }

    private void SetNextDamageLevel()
    {
        var levelWindowPositions =  windowPosition[damageLevel];
        windowWater[damageLevel] = new WaterfallPoolObject[levelWindowPositions.Length];
        var startAngle = 0;
        for (var i = 0; i < levelWindowPositions.Length; i++)
        {
            var angle = startAngle + (i / 2) * 90;
            windowWater[damageLevel][i] = CityData.Instance.waterfallPoolService.GetFreeElement();
            windowWater[damageLevel][i].transform.rotation = Quaternion.Euler(0, angle, 0);
            windowWater[damageLevel][i].transform.position = levelWindowPositions[i];
        }
        damageLevel++;
    }

    private void SetPreviousDamageLevel()
    {
        damageLevel--;
        var levelWindowPositions = windowPosition[damageLevel];
        for (var i = 0; i < levelWindowPositions.Length; i++)
            if (windowWater[damageLevel][i] != default)
                windowWater[damageLevel][i].ReturnToPool();
    }

    private void GenerateWindowPosition()
    {
        var windowCount = 8;
        var sizeX = mesh.bounds.size.x;
        var sizeZ = mesh.bounds.size.z;
        for (var i = 0; i < levelCount; i++)
        {
            var centerLevel = groundCenter + Vector3.up * floorHeight * i;

            windowPosition[i] = new Vector3[windowCount];
            windowPosition[i][0] = new Vector3(centerLevel.x - sizeX / 4, centerLevel.y + windowOffset, centerLevel.z + mesh.bounds.extents.z);
            windowPosition[i][1] = new Vector3(centerLevel.x + sizeX / 4, centerLevel.y + windowOffset, centerLevel.z + mesh.bounds.extents.z);
            windowPosition[i][2] = new Vector3(centerLevel.x + mesh.bounds.extents.x, centerLevel.y + windowOffset, centerLevel.z + sizeZ / 4);
            windowPosition[i][3] = new Vector3(centerLevel.x + mesh.bounds.extents.x, centerLevel.y + windowOffset, centerLevel.z - sizeZ / 4);
            windowPosition[i][4] = new Vector3(centerLevel.x + sizeX / 4, centerLevel.y + windowOffset, centerLevel.z - mesh.bounds.extents.z);
            windowPosition[i][5] = new Vector3(centerLevel.x - sizeX / 4, centerLevel.y + windowOffset, centerLevel.z - mesh.bounds.extents.z);
            windowPosition[i][6] = new Vector3(centerLevel.x - mesh.bounds.extents.x, centerLevel.y + windowOffset, centerLevel.z - sizeZ / 4);
            windowPosition[i][7] = new Vector3(centerLevel.x - mesh.bounds.extents.x, centerLevel.y + windowOffset, centerLevel.z + sizeZ / 4);
        }
    }

    private void OnDrawGizmos()
    {
        for(var i = 0; i < levelCount; i++)
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
