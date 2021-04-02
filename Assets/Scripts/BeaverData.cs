using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaverData : Singleton<BeaverData>
{
    [SerializeField] private GameObject beaverPrefab;
    [SerializeField] private int maxBeaverCount;
    [SerializeField] private float beaverSpeed;
    internal int MaxBeaverCount { get { return maxBeaverCount; } }
    internal GameObject BeaverPrefab { get { return beaverPrefab; } }
    internal float BeaverSpeed { get { return beaverSpeed; } }
    internal Vector3 TargetPosition { get; set; }
    internal int availableBeavers { get; set; }
    internal int busyBeavers { get; set; }

    internal GameObject[] BeaversPool { get; set; }
    internal BeaverStateHandler[] BeaversStateHandler { get; set; }
    
}
