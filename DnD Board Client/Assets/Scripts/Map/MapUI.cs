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

    public void OnStopPlacingUnitsClick()
    {
        _mapManager.StopPlaceUnitMode();
        StopPlaceMode.gameObject.SetActive(false);
        PlaceUnitMode.gameObject.SetActive(true);
    }
    public void OnPlaceEnemyClick()
    {
        _mapManager.PlaceUnit("Enemy");
        StopPlaceMode.gameObject.SetActive(true);
        PlaceUnitMode.gameObject.SetActive(false);
    }
    public void OnPlaceJackClick()
    {
        BaseUnit unit1 = new BaseUnit()
        {
            unitName = "Jack",
            unitType = "Wizard"
        };
        BaseUnit unit2 = new BaseUnit()
        {
            unitName = "Harry",
            unitType = "Barbarian"
        };
        BaseUnit unit3 = new BaseUnit()
        {
            unitName = "Beth",
            unitType = "Druid"
        };
        var testList = new List<BaseUnit>();
        testList.Add(unit1);
        testList.Add(unit2);
        testList.Add(unit3);
        var testListToJson = JsonConvert.SerializeObject(testList);
        UnitLoader.UnitLoaderInstance.LoadUnits(testListToJson);
        var test = UnitManager.UnitManagerInstance.GetAllUnits();
        foreach(var unit in test)
        {
            Debug.Log(unit.unitName);
        }
        _mapManager.PlaceUnit("Jack");
    }
    public void OnPlaceBethClick()
    {
        _mapManager.PlaceUnit("Beth");
    }
    public void OnPlaceHarryClick()
    {
        _mapManager.PlaceUnit("Harry");
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
