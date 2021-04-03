using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeaverSettrings", menuName = "BeaverGame/Beaver", order = 2)]
public class BeaverSettings : ScriptableObject
{
    [Header("Свойства бобрика")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Менеджмент бобриков")]
    [SerializeField] private int maxBeaverCount;
    [SerializeField] private int startPrice = 5;
    [SerializeField] private int priceStep = 5;

    internal int StartPrice { get { return startPrice; } }
    internal int PriceStep { get { return priceStep; } }
    internal int MaxBeaverCount { get { return maxBeaverCount; } }
    internal float RunSpeed { get { return runSpeed; } }
    internal float WalkSpeed { get { return walkSpeed; } }
    internal float RotationSpeed { get { return rotationSpeed; } }
}
