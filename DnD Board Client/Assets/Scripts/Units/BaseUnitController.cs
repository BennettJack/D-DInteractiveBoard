using System;
using DefaultNamespace.Commands;
using Scriptable_Objects.Units.BaseUnits;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUnitController : MonoBehaviour
{
    public TMP_Text namePlate;
    public IBaseUnit BaseUnit;
    public Vector3Int position;
    public int movementRemaining;
    public SelectUnitOnMapCommand selectUnitOnMapCommand;
    
    public SpriteRenderer BodyRenderer;

    private void Awake()
    {
        selectUnitOnMapCommand = new SelectUnitOnMapCommand();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Move(){}
    public void OnSelectUnit()
    {
        Debug.Log(selectUnitOnMapCommand);
        selectUnitOnMapCommand?.Execute(gameObject);
    }
    
}
