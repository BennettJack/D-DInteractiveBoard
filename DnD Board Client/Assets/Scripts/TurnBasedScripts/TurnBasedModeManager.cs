using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.Units;
using Map;
using UnityEngine;

namespace DefaultNamespace.TurnBasedScripts
{
    public class TurnBasedModeManager : MonoBehaviour
    {
        public static TurnBasedModeManager Instance;

        private List<KeyValuePair<string, int>> _initiative;
        private int _currentInitiativeIndex;
        
        public UnitTurn UnitTurn;
        public GameObject endTurnButton;
        public GameObject endCombatButton;
        public bool IsTurnBasedMode;
        private string _currentUnitName;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (IsTurnBasedMode && !endTurnButton.activeInHierarchy)
            {
                endCombatButton.SetActive(true);
                endTurnButton.SetActive(true);
            }
            else if(!IsTurnBasedMode)
            {
                endCombatButton.SetActive(false);
            }
        }

        public void SetInitiative(Dictionary<string, int> initiativeRolls)
        {
            IsTurnBasedMode = true;
            _initiative = initiativeRolls.OrderByDescending(key => key.Value).ToList();
            
        }

        public void StartCombat()
        {
            StartTurn(_initiative[_currentInitiativeIndex].Key);
            Debug.Log("Starting Combat");
        }

        public string  GetCurrentTurn()
        {
            return _currentUnitName;
        }
        private void StartTurn(string unitName)
        {
            UnitTurn = new UnitTurn(unitName);
            _currentUnitName = unitName;
            Debug.Log($"Starting turn: {unitName}");
            var startTile = MapManager.MapManagerInstance.GetUnitPosition(unitName);
            Debug.Log($"we are at {startTile}");
            MovementManager.Instance.ClearData();
            MovementManager.Instance.ClearOverlayTiles();
            MovementManager.Instance.BeginPlayerMovement(startTile);
        }
        public void EndTurn()
        {
            _currentInitiativeIndex += 1;
            if (_currentInitiativeIndex >= _initiative.Count)
            {
                _currentInitiativeIndex = 0;
                StartTurn(_initiative[_currentInitiativeIndex].Key);
            }
            else
            {
                StartTurn(_initiative[_currentInitiativeIndex].Key);
            }
            
            
        }

        public void EndTurnBasedMode()
        {
            _initiative.Clear();
            endTurnButton.SetActive(false);
            UnitTurn = null;
            _currentUnitName = null;
            _currentInitiativeIndex = 0;
            IsTurnBasedMode = false;
        }

        public void RemoveUnitFromInitiative(string unitName)
        {
           var unit = _initiative.FirstOrDefault(unit => unit.Key == unitName);
           _initiative.Remove(unit);
        }
    }
}

