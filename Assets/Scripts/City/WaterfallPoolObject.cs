using UnityEngine;

public class WaterfallPoolObject : MonoBehaviour, IPoolObject
{
    private void Awake()
    {
        gameObject.transform.position = Vector3.down * 5;
        gameObject.transform.rotation = Quaternion.identity;
    }

    public void ReturnToPool()
    {
        gameObject.transform.position = Vector3.down * 5;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
