using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects.Units.BaseUnits;
using UnityEngine;
namespace DefaultNamespace
{
    public class UnitSelectorController : MonoBehaviour
    {
        public GameObject listCardPrefab;
        private List<GameObject> _instantiatedButtons;
        private Transform _parentTransform;

        private void Awake()
        {
            //change this to new prefab later 
            listCardPrefab = Resources.Load("Prefabs/UIPrefabs/UnitSelectButton") as GameObject;
            
            Transform[] children = GetComponentsInChildren<Transform>();
            _parentTransform = children.FirstOrDefault( c => c.name == "Content");
            
        }
        
        public void PopulateContentPanel(List<GameObject> cards)
        {

            foreach (var card in cards)
            {
                Debug.Log(gameObject.name);
                card.SetActive(true);
            }
        }

        void SelectUnit(IBaseUnit unit)
        {
            Debug.Log("Selecting unit " + unit.unitName);
        }
    }
}