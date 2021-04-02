using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaversController : MonoBehaviour
{
    private Vector3[] positionQueue;
    private Queue<BeaverBehaviour> queue = new Queue<BeaverBehaviour>();
    private void Awake()
    {
        StartCoroutine(WaitToRespawn());

        EventBroker.Attack += ToAttack;
        EventBroker.Steal += ToSteal;
    }
    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(1);
        positionQueue = new Vector3[BeaverData.Instance.MaxBeaverCount];
        for(int i = 0; i< positionQueue.Length; i++)
        {
            positionQueue[i] = BeaverData.Instance.SpawnTransform.position;
            positionQueue[i].x += -i;
            Debug.Log(positionQueue[i]);
        }

        BeaverData.Instance.availableBeavers.Add(BeaverData.Instance.beaverPoolService.GetFreeElement());
        BeaverData.Instance.availableBeavers[0].transform.position = positionQueue[0];
        queue.Enqueue(BeaverData.Instance.availableBeavers[0]);
    }
    internal void ToAttack()
    {
        for(int i = 0;i < BeaverData.Instance.availableBeavers.Count; i++)
        {
            if(BeaverData.Instance.availableBeavers[i].gameObject.activeSelf && 
                BeaverData.Instance.availableBeavers[i].currentState == BeaverBehaviour.State.Queue)
            {
                BeaverData.Instance.availableBeavers[i].ToAttackState(BeaverData.Instance.TargetPosition);
                return;
            }
        }
    }
    internal void ToSteal()
    {
        for (int i = 0; i < BeaverData.Instance.availableBeavers.Count; i++)
        {
            if (BeaverData.Instance.availableBeavers[i].gameObject.activeSelf &&
                BeaverData.Instance.availableBeavers[i].currentState == BeaverBehaviour.State.Queue)
            {
                BeaverData.Instance.availableBeavers[i].ToStealState(BeaverData.Instance.TargetPosition);
                return;
            }
        }
    }
}
