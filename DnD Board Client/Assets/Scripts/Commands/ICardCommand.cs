using DefaultNamespace.CampaignSetup;

namespace DefaultNamespace.Commands
{
    public interface ICardCommand
    {
        void Execute(ICardData cardData);
    }
}