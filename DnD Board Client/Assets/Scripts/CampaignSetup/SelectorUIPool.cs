using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.CampaignSetup
{
    public class SelectorUIPool : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public int poolSize = 100;
        
        private Queue<GameObject> _pool = new();


        void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                
            }
        }
    }
}