using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal class BeaversController : MonoBehaviour
{
    [SerializeField] private BeaverSettings beaverSettings;
    [SerializeField] private Transform spawnTransform;

    private int currentPrice;
    internal static int chocolateAmount { get; private set; }
    internal static int CountBeaverInQueue { get { return queue.Count; } }
    internal static int AvailableBeavers { get { return availableBeavers.Count; } }

    internal PoolingService<BeaverBehaviour> beaverPoolService = null;

    
    internal static Vector3 targetPosition { get; set; }
    private static Vector3 spawnOffset;

    private static Queue<BeaverBehaviour> queue = new Queue<BeaverBehaviour>();
    private static List<BeaverBehaviour> availableBeavers = new List<BeaverBehaviour>();
    internal static Vector3 spawnPoint { get; private set; }
    private Rigidbody _rigidbody;
    private void Awake()
    {
        currentPrice = beaverSettings.StartPrice;
        EventBroker.UpdateChocolateInvoke(chocolateAmount);
        EventBroker.UpdatePriceInvoke(currentPrice);
        
        StartCoroutine(WaitToRespawn());
        spawnOffset = Vector3.zero;
        spawnPoint = spawnTransform.position;
        EventBroker.Attack += ToAttack;
        EventBroker.Steal += ToSteal;
        EventBroker.UpdateGUI += UpdateGUE;
        EventBroker.BuyBeaver += BuyBeaver;
        _rigidbody = GetComponent<Rigidbody>();
    }
    private IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(1);
        availableBeavers.Add(beaverPoolService.GetFreeElement());
        availableBeavers[0].transform.position = SpawnPosition();
        queue.Enqueue(availableBeavers[0]);
    }
    private void UpdateGUE()
    {
        EventBroker.UpdateChocolateInvoke(chocolateAmount);
        EventBroker.UpdatePriceInvoke(currentPrice);
    }
    private void OnDestroy()
    {
        chocolateAmount = 0;
        EventBroker.BuyBeaver -= BuyBeaver;
        EventBroker.UpdateGUI -= UpdateGUE;
        EventBroker.Attack -= ToAttack;
        EventBroker.Steal -= ToSteal;
        availableBeavers.Clear();
        queue.Clear();
    }
    internal void ToAttack()
    {
        if (queue.Count != 0)
        {
            queue.Dequeue().ToAttackState(targetPosition);
            QueueOffset();
        }
    }
    internal void ToSteal()
    {
        if (queue.Count != 0)
        {
            queue.Dequeue().ToStealState(targetPosition);
            QueueOffset();
        }
    }
    private void Update()
    {
        spawnPoint = spawnTransform.position;
        if (_rigidbody.IsSleeping())
            _rigidbody.WakeUp();
    }
    public void BuyBeaver()
    {
        if (availableBeavers.Count < beaverSettings.MaxBeaverCount && chocolateAmount >= currentPrice) 
        {
            chocolateAmount -= currentPrice;
            currentPrice += beaverSettings.PriceStep;
            EventBroker.UpdateGUIInvoke();
            availableBeavers.Add(beaverPoolService.GetFreeElement());
            queue.Enqueue(availableBeavers[availableBeavers.Count - 1]);
            availableBeavers[availableBeavers.Count - 1].transform.position = SpawnPosition();
            
        }
    }
    internal static void AddToQueue(BeaverBehaviour beaver)
    {
        queue.Enqueue(beaver);
    }
    internal static Vector3 SpawnPosition()
    {
        spawnOffset.x = queue.Count == 0? queue.Count : (-queue.Count + 1 )* 1.5f;
        return spawnPoint + spawnOffset;
    }
    internal static Vector3 QueuePosition()
    {
        spawnOffset.x = queue.Count == 0 ? queue.Count : (-queue.Count) * 1.5f;
        return spawnPoint + spawnOffset;
    }
    internal static void QueueOffset()
    {
        foreach(var beaver in queue)
        {
            beaver.ToQueueState();
        }
    }
    internal static void RemoveFromAvailableList(BeaverBehaviour beaver)
    {
        availableBeavers.Remove(beaver);
    }
    internal static void AddChocolateToStock(int count)
    {
        
        chocolateAmount += count;
        EventBroker.UpdateGUIInvoke();
    }
}
