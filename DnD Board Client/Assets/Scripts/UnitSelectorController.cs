using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Units;
using Scriptable_Objects.Units.BaseUnits;
using Scriptable_Objects.Units.BaseUnits.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UnitSelectorController : MonoBehaviour
    {
        public GameObject unitSelectBtnPrefab;
        private List<GameObject> _instantiatedButtons;
        private Transform _parentTransform;

        private void Awake()
        {
            unitSelectBtnPrefab = Resources.Load("Prefabs/UIPrefabs/UnitSelectButton") as GameObject;
            
            Transform[] children = GetComponentsInChildren<Transform>();
            _parentTransform = children.FirstOrDefault( c => c.name == "Content");
            
        }
        
        public void PopulateContentPanel(List<IBaseUnit> units)
        {
            units.Add(
                new Barbarian()
                {
                    unitName = "Barb",
                    unitType = "Barbarian",
                });
            foreach (var unit in units)
            {
                Debug.Log(gameObject.name);
                GameObject btnObj = Instantiate(unitSelectBtnPrefab, _parentTransform);
                var textComponent = btnObj.GetComponentInChildren<TextMeshPro>();
                try
                {
                    textComponent.text = unit.unitName;
                }
                catch (NullReferenceException e)
                {
                    Debug.Log("text is null");
                }
                
                var btn = btnObj.GetComponent<Button>();
                try
                {
                    btn.onClick.AddListener(delegate { SelectUnit(unit); });
                }
                catch (NullReferenceException e)
                {
                    Debug.Log("button is null");
                }
            }
        }

        void SelectUnit(IBaseUnit unit)
        {
            Debug.Log("Selecting unit " + unit.unitName);
        }
    }
}