using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverPool : MonoBehaviour
{
    [SerializeField]
    private BeaverBehaviour beaverPrefab = null;
    [SerializeField] private BeaversController beaversController;


    private void Start()
    {
        if (beaverPrefab != null)
            beaversController.beaverPoolService = new PoolingService<BeaverBehaviour>(beaverPrefab, BeaverData.Instance.MaxBeaverCount, transform, true);
    }
}
