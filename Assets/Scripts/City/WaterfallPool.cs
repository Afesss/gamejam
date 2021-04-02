using UnityEngine;

public class WaterfallPool : MonoBehaviour
{
    [Tooltip("������ ������� ���������� ����")]
    [SerializeField]
    private WaterfallPoolObject waterfallPrefab = null;

    [Tooltip("����������� ������ ����")]
    [SerializeField]
    private int size;

    private void Start()
    {
        if (waterfallPrefab != null)
            CityData.Instance.waterfallPoolService = new PoolingService<WaterfallPoolObject>(waterfallPrefab, size, transform, true);
    }

}