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

    private int[] remainHousesToPlace;
    private int freeHouseTypesCount;

    private void Awake()
    {
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
        freeHouseTypesCount = config.HouseSets.Length;

        var count = config.CityColsCount * config.CityRowsCount;
        remainHousesToPlace = new int[config.HouseSets.Length];
        var sum = 0;
        for(var i = 0; i < config.HouseSets.Length - 1; i++)
        {
            remainHousesToPlace[i] = (int) ((float)config.HouseSets[i].CountPercent * count / 100);
            sum += remainHousesToPlace[i];
        }
        remainHousesToPlace[config.HouseSets.Length - 1] = count - sum;
    }

    private void InstantiateHouse(int col, int row, int index)
    {
        var position = new Vector3(col * config.CellSize.x, 0, row * config.CellSize.y)
            - new Vector3(config.CellSize.x * config.CityColsCount * 0.5f, 0, config.CellSize.y * config.CityRowsCount * 0.5f);

        var obj = GameObject.Instantiate(config.HouseSets[index].Prefab, position, Quaternion.identity, transform);

        SetRendomMaterialSet(ref obj, config.HouseSets[index].PrefabMaterialsCount, out MaterialSet materialSet);

        if (config.HouseSets[index].HasRoof)
            AddRandomRoof(ref obj, materialSet);
    }

    private void AddRandomRoof(ref GameObject house, MaterialSet materialSet)
    {
        var houseRenderer = house.GetComponentInChildren<MeshRenderer>();
        if (houseRenderer != null)
        {
            var roof = config.RoofObjects[UnityEngine.Random.Range(0, config.RoofObjects.Length)];
            var position = new Vector3(house.transform.position.x, houseRenderer.bounds.size.y, house.transform.position.z);
            var obj = GameObject.Instantiate(roof, position, Quaternion.identity, house.transform);

            var renderer = obj.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            {
                Material[] mats = renderer.materials;
                mats[0] = materialSet.materials[0];
                renderer.materials = mats;
            }
        }
    }

    private void SetRendomMaterialSet(ref GameObject house, int modelMaterialCount, out MaterialSet materialSet)
    {
        materialSet = config.MaterialSets[UnityEngine.Random.Range(0, config.MaterialSets.Length)];
        var renderer = house.GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            Material[] mats = renderer.materials;
            for (var i = 0; i < modelMaterialCount; i++)
                if (mats[i] != null && materialSet.materials[i] != null)
                    mats[i] = materialSet.materials[i];
            renderer.materials = mats;
        }
    }



}