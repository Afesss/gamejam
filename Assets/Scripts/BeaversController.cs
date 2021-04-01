using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BeaversController : MonoBehaviour
{
    private int maxBeaverCount;
    private void Awake()
    {
        maxBeaverCount = BeaverData.Instance.MaxBeaverCount;
        BeaverData.Instance.BeaversPool = new GameObject[maxBeaverCount];
        BeaverData.Instance.BeaversStateHandler = new BeaverStateHandler[maxBeaverCount];
        for (int i = 0; i < maxBeaverCount; i++)
        {
            BeaverData.Instance.BeaversPool[i] = Instantiate(BeaverData.Instance.BeaverPrefab, transform.position,
                 Quaternion.identity, transform);
            BeaverData.Instance.BeaversStateHandler[i] = BeaverData.Instance.BeaversPool[i].GetComponent<BeaverStateHandler>();
            BeaverData.Instance.BeaversPool[i].SetActive(false);
        }
        BeaverData.Instance.BeaversPool[0].SetActive(true);
        
        BeaverData.Instance.availableBeavers = 1;

        EventBroker.Attack += ToAttack;
    }
    internal void ToAttack()
    {
        for(int i=0;i< maxBeaverCount; i++)
        {
            if(BeaverData.Instance.BeaversPool[i].activeSelf && !BeaverData.Instance.BeaversStateHandler[i].attack)
            {
                BeaverData.Instance.BeaversStateHandler[i].ToAttackState();
                return;
            }
        }
    }
}
