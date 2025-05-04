using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MapEditorUI : MonoBehaviour
{
    public TMP_InputField HorizontalTileCountInput;
    public TMP_InputField VerticalTileCountInput;
    public TMP_Text SetTileCountText;
    public Button SetTileCountButton;

    public TMP_InputField MapNameInput;
    //Separation of concerns, UI shouldnt be changing tilemap directly, instead tell map editor to do it
    private MapEditorManager _mapEditorManager;

    private void Awake()
    {
        SetTileCountButton.enabled = false;
    }

    public void Start()
    {
        _mapEditorManager = MapEditorManager.MapEditorManagerInstance;
        MapNameInput.onValueChanged.AddListener(delegate { OnMapNameUpdate(MapNameInput.text); });
    }
    
    public void SelectMap()
    {
        _mapEditorManager.SetMapImage("Maps/IMG_4164");
        SetTileCountButton.enabled = true;
    }

    public void SetTileCounts()
    {
        var horizontalCount = int.Parse(HorizontalTileCountInput.text);
        var verticalTileCount = int.Parse(VerticalTileCountInput.text);
        _mapEditorManager.SetTileCounts(verticalTileCount, horizontalCount);
        
        if (_mapEditorManager.TileMapHasTiles())
        {
            SetTileCountText.text = "Update Tile Counts";
        }
    }

    public void OnPaintWallsClick()
    {
        _mapEditorManager.EnableWallPaintMode();
    }

    public void OnEditTilesClick()
    {
        
    }

    public void OnPlaceUnitsClick()
    {
        
    }
    
    
    public void OnSaveClick()
    {
 
        
        _mapEditorManager.SaveMap();
        
    }
    
    public void OnMapNameUpdate(string mapName)
    {
        _mapEditorManager.SetMapName(mapName);
    }
    
}
