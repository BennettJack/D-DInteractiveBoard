using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DefaultNamespace.CampaignSetup;
using DefaultNamespace.Commands;
using DefaultNamespace.GenericUI;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;
namespace DefaultNamespace
{
    public class UnitSelectorController : MonoBehaviour
    {
        
        private List<GameObject> _instantiatedCards = new();
        private Transform _parentTransform;

        private void Awake()
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            _parentTransform = children.FirstOrDefault( c => c.name == "Content");
        }
        
        public void PopulateUnitList(List<ICardData> cardData)
        {
            ResetCardList();
            foreach (var card in cardData)
            {
                GameObject cardObject = CardUIPool.Instance.GetCard();
                cardObject.transform.SetParent(_parentTransform, false);
                cardObject.SetActive(true);

                var baseNameCard = cardObject.GetComponent<BaseNameCard>();
                baseNameCard.Setup(card);
                
                _instantiatedCards.Add(cardObject);
                
            }
        }
        
        public void PopulateUnitList(List<ICardData> cardData, ICardCommand command)
        {
            ResetCardList();
            foreach (var card in cardData)
            {
                GameObject cardObject = CardUIPool.Instance.GetCard();
                cardObject.transform.SetParent(_parentTransform, false);
                cardObject.SetActive(true);
                
                var baseNameCard = cardObject.GetComponent<BaseNameCard>();
                baseNameCard.Setup(card, command);
                
                _instantiatedCards.Add(cardObject);
                
            }
        }

        void SelectUnit(IBaseUnit unit)
        {
            Debug.Log("Selecting unit " + unit.unitName);
        }

        void ResetCardList()
        {
            foreach (var card in _instantiatedCards)
            {
                CardUIPool.Instance.ReturnCard(card);
            }
            _instantiatedCards.Clear();
        }
    }
}