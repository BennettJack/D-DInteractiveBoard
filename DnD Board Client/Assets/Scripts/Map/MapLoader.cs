using System;
using System.IO;
using UnityEngine;

namespace Map
{
    public class MapLoader : MonoBehaviour
    {
        public static MapLoader Instance;
        public SpriteRenderer mapSpriteRenderer;
        private void Awake()
        {
            Instance = this;
        }
        
        public void LoadMapFromFile(string mapFileName)
        {
            MapTileMapManager.MapTileMapManagerInstance.ClearAllTilemaps();
            var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/DnD Board Client/Maps";
            var filePath = Path.Combine(documentsLocation, mapFileName + ".json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var mapData = JsonUtility.FromJson<MapData>(json);
        
                GenerateMap(mapData);
                MapManager.MapManagerInstance.mapData = mapData;
            }
        }

        private void GenerateMap(MapData mapData)
        {
            var sprite = Resources.Load<Sprite>(mapData.MapFileName);
            mapSpriteRenderer.sprite = sprite;
            mapSpriteRenderer.transform.position = new Vector3(0, 0, 5);

            var bottomLeftCorner = mapSpriteRenderer.transform.TransformPoint(mapSpriteRenderer.sprite.bounds.min);

            MapTileMapManager.MapTileMapManagerInstance.LoadFromData(mapData, "map");
            MapTileMapManager.MapTileMapManagerInstance.SnapToMapImage(bottomLeftCorner);
        }
    }
}