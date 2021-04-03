public class CityData : Singleton<CityData>
{
    public PoolingService<WaterfallPoolObject> waterfallPoolService = null;

    public WorldWaterLevel worldWaterLevel = null;
}
