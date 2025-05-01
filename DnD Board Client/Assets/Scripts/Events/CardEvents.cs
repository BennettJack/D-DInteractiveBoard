using System;
using DefaultNamespace.CampaignSetup;
using UnityEngine;

namespace DefaultNamespace.Events
{
    public class CardEvents
    {
        public static event Action<ICardData, MonoBehaviour> OnCardTransfer;

        public static void RaiseCardTransfer(ICardData cardData, MonoBehaviour targetScript)
        {
            OnCardTransfer?.Invoke(cardData, targetScript);
        }
    }
}

