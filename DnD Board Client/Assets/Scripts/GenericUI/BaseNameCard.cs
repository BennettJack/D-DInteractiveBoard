using DefaultNamespace.CampaignSetup;
using DefaultNamespace.Commands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.GenericUI
{
    public class BaseNameCard : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI nameText;
        
        private ICardData _cardData;
        private ICardCommand _command;

        public void Setup(ICardData cardData, ICardCommand command)
        {
            _cardData = cardData;
            _command = command;
            
            nameText.text = _cardData.Name;
        }
        
        
        public void Setup(ICardData cardData)
        {
            _cardData = cardData;
            gameObject.name = _cardData.Name;
            nameText.text = _cardData.Name;
        }
        public void UpdateCommand(ICardCommand command)
        {
            _command = command;
        }

        public void ExecuteCommand()
        {
            _command.Execute(_cardData);
        }
    }
}