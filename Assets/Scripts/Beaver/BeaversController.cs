using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaversController : MonoBehaviour
{
    
    private int maxBeaverCount;
    private void Start()
    {
        StartCoroutine(WaitToRespawn());

        EventBroker.Attack += ToAttack;
        EventBroker.Steal += ToSteal;
    }
    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(1);
        BeaverData.Instance.availableBeavers.Add(BeaverData.Instance.beaverPoolService.GetFreeElement());
        BeaverData.Instance.availableBeavers[0].transform.position = BeaverData.Instance.SpawnTransform.position;
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
