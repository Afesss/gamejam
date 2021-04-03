using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaversController : MonoBehaviour
{

    [SerializeField] private Transform spawnTransform;
    [SerializeField] private int maxBeaverCount;

    internal PoolingService<BeaverBehaviour> beaverPoolService = null;
    internal static Vector3 targetPosition { get; set; }
    private static Vector3 spawnOffset;
    internal static int queueCount { get { return queue.Count; } }
    private static Queue<BeaverBehaviour> queue = new Queue<BeaverBehaviour>();
    private static List<BeaverBehaviour> availableBeavers = new List<BeaverBehaviour>();
    internal static Vector3 spawnPoint { get; private set; }
    private void Start()
    {
        StartCoroutine(WaitToRespawn());
        spawnOffset = Vector3.zero;
        spawnPoint = spawnTransform.position;
        EventBroker.Attack += ToAttack;
        EventBroker.Steal += ToSteal;
    }
    private IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(1);
        availableBeavers.Add(beaverPoolService.GetFreeElement());
        availableBeavers[0].transform.position = SpawnPosition();
        queue.Enqueue(availableBeavers[0]);

    }
    private void OnDestroy()
    {
        EventBroker.Attack -= ToAttack;
        EventBroker.Steal -= ToSteal;
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
    
    internal void Spawn()
    {
        if (availableBeavers.Count < maxBeaverCount) 
        {
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
}
