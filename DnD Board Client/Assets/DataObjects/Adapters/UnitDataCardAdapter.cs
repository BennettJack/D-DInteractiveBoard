using DefaultNamespace.CampaignSetup;
using Scriptable_Objects.Units.BaseUnits;

namespace DataObjects.Adapters
{
    public class UnitDataCardAdapter : ICardData
    {
        private IBaseUnit _baseUnit;

        public UnitDataCardAdapter(IBaseUnit baseUnit)
        {
            _baseUnit = baseUnit;
        }

        public int Id => _baseUnit.unitName.GetHashCode();
        public string Name => _baseUnit.unitName;
    }
}