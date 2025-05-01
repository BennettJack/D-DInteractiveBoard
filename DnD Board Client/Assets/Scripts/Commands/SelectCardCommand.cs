using System.Windows.Input;
using DefaultNamespace.CampaignSetup;
using DefaultNamespace.GenericUI;
using UnityEngine;

namespace DefaultNamespace.Commands
{
    public class SelectCardCommand : ICardCommand
    {
        private UnitSelectorController _targetScript;

        public SelectCardCommand(UnitSelectorController targetScript)
        {
            _targetScript = targetScript;
        }
        public void Execute(ICardData card)
        {
        }

    }
}