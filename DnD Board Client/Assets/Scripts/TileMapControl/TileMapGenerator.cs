using System;
using Map_Editor;
using UnityEngine;

namespace TileMapControl
{
    public class TileMapGenerator : MonoBehaviour
    {
        public static TileMapGenerator TileMapGeneratorInstance;
        private MapEditorTileMapManger _mapEditorTileMapManger;

        private void Awake()
        {
            TileMapGeneratorInstance = this;
        }

        void Start()
        {
            _mapEditorTileMapManger = MapEditorTileMapManger.MapEditorTileMapMangerInstance;
        }

        public void GenerateVisionTileMap(int width, int height, float tileWidth, float tileHeight)
        {
            
        }
        
        public void GenerateGroundTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GenerateOverlayTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GeneratePreviewTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GenerateWallTileMap(int width, int height, float tileWidth, float tileHeight){}
    }
}