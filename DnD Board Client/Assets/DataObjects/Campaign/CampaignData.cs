using System.Collections.Generic;
using Scriptable_Objects.Units.BaseUnits;

namespace DataObjects.Campaign
{
    public class CampaignData
    {
        public string CampaignName { get; set; }
        public List<string> Maps { get; set; }
        public List<IBaseUnit> PlayerUnits { get; set; }
        public List<IBaseUnit> EnemyUnits { get; set; }
    }
}