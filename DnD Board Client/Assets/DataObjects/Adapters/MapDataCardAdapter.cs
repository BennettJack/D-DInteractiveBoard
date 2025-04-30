using DefaultNamespace.CampaignSetup;
using UnityEngine.UI;

namespace DataObjects.Adapters
{
    public class MapDataCardAdapter : ICardData
    {
        private MapData _mapData;

        public MapDataCardAdapter(MapData mapData)
        {
            _mapData = mapData;
        }

        public int Id => _mapData.MapFileName.GetHashCode();
        public string Name => _mapData.MapFileName;
    }
}