using UnityEngine;

public class WaterfallPool : MonoBehaviour
{
    [Tooltip("Префаб объекта вытекающей воды")]
    [SerializeField]
    private WaterfallPoolObject waterfallPrefab = null;

    [Tooltip("Изначальный размер пула")]
    [SerializeField]
    private int size;

    private void Start()
    {
        if (waterfallPrefab != null)
            CityData.Instance.waterfallPoolService = new PoolingService<WaterfallPoolObject>(waterfallPrefab, size, transform, true);
    }

}