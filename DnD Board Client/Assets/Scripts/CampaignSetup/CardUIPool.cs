using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.CampaignSetup
{
    public class CardUIPool : MonoBehaviour
    {
        public GameObject cardPrefab;
        public static CardUIPool Instance;
        //This is currently hard coded, but when implemented properly, an awake function
        //can call an API to get the actual count of all units, maps and items etc
        public int poolSize = 100;
        
        private Queue<GameObject> _pool = new();

        private void Awake()
        {   
            Instance = this;
        }
        void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject cardObject = Instantiate(cardPrefab, transform);
                cardObject.SetActive(false);
                _pool.Enqueue(cardObject);
            }
        }

        public GameObject GetCard()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : Instantiate(cardPrefab, transform);
        }

        public void ReturnCard(GameObject cardObject)
        {
            cardObject.SetActive(false);
            _pool.Enqueue(cardObject);
        }
    }
}