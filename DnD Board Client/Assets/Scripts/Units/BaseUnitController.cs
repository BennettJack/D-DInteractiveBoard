using System;
using Scriptable_Objects.Units.BaseUnits;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUnitController : MonoBehaviour
{
    public TMP_Text namePlate;
    public BaseUnit baseUnit { get; private set; }
    public int unitId;
    public Vector3Int position;
    private MapManager _mapManagerInstance;
    public int movementRemaining;
    public int movementSpeed;

    public SpriteRenderer BodyRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Move(){}

    private void Awake()
    {
    }

    private void Start()
    {
        _mapManagerInstance = MapManager.MapManagerInstance;
    }

    void OnMouseDown()
    {
        _mapManagerInstance.currentlySelectedUnit = gameObject;
        _mapManagerInstance.unitSelectDelay = 0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log("colliding");
    }

    public void UpdateVision()
    {
        Debug.Log("updateVision");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector3(transform.position.x, transform.position.y, 0), 1f, transform.forward
            , 12f, LayerMask.GetMask("Vision"));
        
        int count;
        
        
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            
        }
        
    }
    
}
