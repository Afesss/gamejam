using System;
using UnityEngine;

/// <summary>
/// Генератор города
/// </summary>
public class CityGenerator : MonoBehaviour
{
    [Tooltip("Настройки города")]
    [SerializeField]
    private CitySettings config;

    // TODO: Убрать эти поля и везде где они используются - использовать сразу config
    private int cityColsCount;
    private int cityRowsCount;
    private Vector2 CellSize;
    private GameObject[] houseObjects;
    private int[] houseRates;
    // ------------------------------------------------------------

    private int[] remainHousesToPlace;
    private int freeHouseTypesCount;

    private void Awake()
    {
        cityColsCount = config.CityColsCount;
        cityRowsCount = config.CityRowsCount;
        CellSize = config.CellSize;
        houseObjects = config.HouseObjects;
        houseRates = config.HouseRates;

        InitHousesCounts();

        for (var x = 0; x < config.CityColsCount; x++)
        {
            for (var z = 0; z < config.CityRowsCount; z++)
            {
                InstantiateHouse(x, z, GetRandomFreeHouseIndex());
            }
        }
    }

    private int GetRandomFreeHouseIndex()
    {
        Array.Sort(remainHousesToPlace);
        Array.Reverse(remainHousesToPlace);

        var index = UnityEngine.Random.Range(0, freeHouseTypesCount);
        remainHousesToPlace[index] --;
        if (remainHousesToPlace[index] <= 0)
            freeHouseTypesCount--;

        return index;
    }

    private void InitHousesCounts()
    {
        freeHouseTypesCount = config.HouseObjects.Length;

        var count = config.CityColsCount * config.CityRowsCount;
        remainHousesToPlace = new int[config.HouseObjects.Length];
        var sum = 0;
        for(var i = 0; i < config.HouseObjects.Length - 1; i++)
        {
            remainHousesToPlace[i] = (int) ((float)config.HouseRates[i] * count / 100);
            sum += remainHousesToPlace[i];
        }
        remainHousesToPlace[config.HouseObjects.Length - 1] = count - sum;
    }

    private void InstantiateHouse(int col, int row, int index)
    {
        var position = new Vector3(col * config.CellSize.x, 0, row * config.CellSize.y);
        GameObject.Instantiate(config.HouseObjects[index], position, Quaternion.identity, transform);
    }

}
