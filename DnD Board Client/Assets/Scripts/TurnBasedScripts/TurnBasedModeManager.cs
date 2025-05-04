using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.TurnBasedScripts
{
    public class TurnBasedModeManager : MonoBehaviour
    {
        public static TurnBasedModeManager Instance;

        private List<KeyValuePair<string, int>> _initiative;
        private int _currentInitiativeIndex;
        
        public UnitTurn UnitTurn;
        
        public bool IsTurnBasedMode;
        private string _currentUnitName;
        
        private void Awake()
        {
            Instance = this;
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
            Debug.Log($"starting turn: {unitName}");
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
            UnitTurn = null;
            _currentUnitName = null;
            _currentInitiativeIndex = 0;
            IsTurnBasedMode = false;
        }
    }
}