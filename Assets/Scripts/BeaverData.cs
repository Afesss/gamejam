using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverData : Singleton<BeaverData>
{
    
    protected override void Awake()
    {
        base.Awake();
    }
    internal PoolingService<BeaverBehaviour> beaverPoolService = null;
    [SerializeField] private GameObject beaverPrefab;
    [SerializeField] private int maxBeaverCount;
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform spawnTransform;

    
    internal int MaxBeaverCount { get { return maxBeaverCount; } }
    internal GameObject BeaverPrefab { get { return beaverPrefab; } }
    internal float RunSpeed { get { return runSpeed; } }
    internal float WalkSpeed { get { return walkSpeed; } }
    internal float RotationSpeed { get { return rotationSpeed; } }
    internal Vector3 TargetPosition { get; set; }
    internal Transform SpawnTransform { get { return spawnTransform; } }
    internal int busyBeavers { get; set; }

    internal List<BeaverBehaviour> availableBeavers = new List<BeaverBehaviour>();
}
