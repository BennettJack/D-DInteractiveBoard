using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;

namespace DefaultNamespace.Commands
{
    public interface IUnitCommand
    {
        void Execute(GameObject unit);
    }
}