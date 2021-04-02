using System;
using UnityEngine;

/// <summary>
/// ��������� ������
/// </summary>
public class CityGenerator : MonoBehaviour
{
    [Tooltip("���������� ������� ��� �����")]
    [SerializeField]
    private int cityColsCount;

    [Tooltip("���������� ����� ��� �����")]
    [SerializeField]
    private int cityRowsCount;

    [Tooltip("������ ������ �� ������� ����� ���")]
    [SerializeField]
    private Vector2 CellSize;

    [Tooltip("������� �����")]
    [SerializeField]
    private GameObject[] houseObjects;

    [Tooltip("���������� ����������� �����")]
    [SerializeField]
    private int[] houseRates;

    private int[] remainHousesToPlace;
    private int freeHouseTypesCount;


    private void Awake()
    {
        InitHousesCounts();

//        foreach (var value in remainHousesToPlace)
//            Debug.Log(value);

        for (var x = 0; x < cityColsCount; x++)
        {
            for (var z = 0; z < cityRowsCount; z++)
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
        freeHouseTypesCount = houseObjects.Length;

        var count = cityColsCount * cityRowsCount;
        remainHousesToPlace = new int[houseObjects.Length];
        var sum = 0;
        for(var i = 0; i < houseObjects.Length - 1; i++)
        {
            remainHousesToPlace[i] = (int) ((float) houseRates[i] * count / 100);
            sum += remainHousesToPlace[i];
        }
        remainHousesToPlace[houseObjects.Length - 1] = count - sum;
    }

    private void InstantiateHouse(int col, int row, int index)
    {
        var position = new Vector3(col * CellSize.x, 0, row * CellSize.y)
            - new Vector3(CellSize.x * cityColsCount * 0.5f,0,CellSize.y * cityRowsCount * 0.5f);
        GameObject.Instantiate(houseObjects[index], position, Quaternion.identity, transform);
    }

}
