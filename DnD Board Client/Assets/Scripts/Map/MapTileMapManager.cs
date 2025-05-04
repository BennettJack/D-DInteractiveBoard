using System;
using DefaultNamespace;
using TileMapControl;
using UnityEngine;

namespace Map
{
    public class MapTileMapManager : TileMapManager
    {
        public static MapTileMapManager MapTileMapManagerInstance;

        private void Awake()
        {
            MapTileMapManagerInstance = this;
            
            AddTileMapsToDictionary();
        }

        private void Start()
        {
            _tileGallery = TileGallery.TileGalleryInstance;
            _tileMapGenerator = TileMapGenerator.TileMapGeneratorInstance;
            
            
        }

        public void HighlightPossibleMovementPath(Vector3Int position)
        {
            tileMaps["overlay"].SetTile(position, _tileGallery.GetTile("MovementOverlay"));
        }

        public void UpdateGroundTileMap()
        {
            
        }
        
    }
}