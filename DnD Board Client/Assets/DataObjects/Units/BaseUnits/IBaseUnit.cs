using JetBrains.Annotations;

namespace Scriptable_Objects.Units.BaseUnits
{
    public interface IBaseUnit
    {
        string unitType { get; set; }
        string unitName { get; set; }
        
    }
}