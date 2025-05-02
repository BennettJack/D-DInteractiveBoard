using System.Collections.Generic;
using DataObjects.Units;
using Newtonsoft.Json;
using Scriptable_Objects.Units.BaseUnits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    
    private MapManager _mapManager;

    public Button JackButton;
    public Button BethButton;
    public Button HarryButton;
    public TMP_InputField MoveSpeedInputField;
    public Button PlaceUnitMode;
    public Button StopPlaceMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mapManager = MapManager.MapManagerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Jack") != null)
        {
            JackButton.gameObject.SetActive(false);
        }
        else
        {
            JackButton.gameObject.SetActive(true);
        }
        if (GameObject.Find("Beth") != null)
        {
            BethButton.gameObject.SetActive(false);
        }
        else
        {
            BethButton.gameObject.SetActive(true);
        }
        if (GameObject.Find("Harry") != null)
        {
            HarryButton.gameObject.SetActive(false);
        }
        else
        {
            HarryButton.gameObject.SetActive(true);
        }

        if (_mapManager.currentlySelectedUnit != null)
        {
            MoveSpeedInputField.gameObject.SetActive(true);
            MoveSpeedInputField.text = MoveSpeedInputField.text = _mapManager.currentlySelectedUnit.
                GetComponent<BaseUnitController>().movementSpeed.ToString();
        }
        else
        {
            MoveSpeedInputField.gameObject.SetActive(false);
        }
    }

    public void OnPlaceEnemyUnitModeClick()
    {
        
    }

    public void OnMoveSpeedUpdate()
    {
        _mapManager.UpdateUnitMoveSpeed(int.Parse(MoveSpeedInputField.text));
    }

    public void DestroyUnit()
    {
        _mapManager.DestroySelectedUnit();
    }
}
