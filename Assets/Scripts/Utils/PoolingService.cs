using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolingService<T> where T : MonoBehaviour, IPoolObject
{
    public T prefab { get; }
    public bool isAutoExpand { get; set; }
    public Transform container { get; }

    private List<T> pool;

    public PoolingService(T prefab, int count, Transform container, bool isAutoExpand = false)
    {
        this.prefab = prefab;
        this.container = container;
        this.CreatePool(count);
        this.isAutoExpand = isAutoExpand;
    }

    private void CreatePool(int count)
    {
        pool = new List<T>();

        for (int i = 0; i < count; i++)
            CreateObject();
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = GameObject.Instantiate<T>(prefab, container) as T;
        createdObject.gameObject.SetActive(isActiveByDefault);
        this.pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
            return element;

        if (isAutoExpand)
            return CreateObject(true);

        throw new Exception($"В пуле нет свободного элемента типа {typeof(T)}");
    }
}
