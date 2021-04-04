using UnityEngine;
public class CityData : Singleton<CityData>
{
    public PoolingService<WaterfallPoolObject> waterfallPoolService = null;

    public Transform waterTransform;
    public WorldWaterLevel worldWaterLevel = null;
}
