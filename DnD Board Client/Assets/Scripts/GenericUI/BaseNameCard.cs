using DefaultNamespace.CampaignSetup;
using DefaultNamespace.Commands;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.GenericUI
{
    public class BaseNameCard : MonoBehaviour
    {
        public TextMeshPro nameText;
        
        private ICardData _cardData;
        private ICardCommand _command;

        public void Setup(ICardData cardData, ICardCommand command)
        {
            _cardData = cardData;
            _command = command;
            
            nameText.text = _cardData.Name;
        }

        public void UpdateCommand(ICardCommand command)
        {
            _command = command;
        }

        public void OnMouseDown()
        {
            _command.Execute( _cardData );
        }
    }
}