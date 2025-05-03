using DefaultNamespace.CampaignSetup;

namespace DefaultNamespace.Commands
{
    public class SelectUnitToPlaceCommand : ICardCommand
    {
        public void Execute(ICardData card)
        {
            MapManager.MapManagerInstance.SetUnitToPlace(card.Name);
        }
    }
}