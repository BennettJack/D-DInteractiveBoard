using System;
using System.Linq;
using Map_Editor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMapControl
{
    public class TileMapGenerator : MonoBehaviour
    {
        public static TileMapGenerator TileMapGeneratorInstance;
        private TileGallery _tileGallery;
        private MapEditorTileMapManger _mapEditorTileMapManger;

        private void Awake()
        {
            TileMapGeneratorInstance = this;
        }

        void Start()
        {
            _mapEditorTileMapManger = MapEditorTileMapManger.MapEditorTileMapMangerInstance;
            _tileGallery = TileGallery.TileGalleryInstance;
        }

        public Tilemap GenerateVisionTileMap(int width, int height, float tileWidth, float tileHeight)
        {
            var tilemap = new Tilemap();
            
            return tilemap;
        }
        
        public Tilemap GenerateVisionTileMap(int width, int height, MapData mapData, Tilemap tileMap)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (mapData.DiscoveredVisionTiles.FirstOrDefault(tile => tile.position == new Vector3Int(i, j, 0)) != null)
                    {
                        Debug.Log("Vision Tile Found");
                        tileMap.SetTile(new Vector3Int(i, j, 0), Instantiate(_tileGallery.GetTile("FullVision")));
                            
                    }
                    else
                    {
                        
                        tileMap.SetTile(new Vector3Int(i, j, 0), Instantiate(_tileGallery.GetTile("NoVision")));
                        
                    }
                }
            }

            return tileMap;
        }
        
        public void GenerateGroundTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GenerateGroundTileMap(int width, int height, float tileWidth, float tileHeight, MapData mapData){}
        
        public void GenerateOverlayTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GenerateOverlayTileMap(int width, int height, float tileWidth, float tileHeight, MapData mapData){}
        
        public void GeneratePreviewTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GeneratePreviewTileMap(int width, int height, float tileWidth, float tileHeight, MapData mapData){}
        
        public void GenerateWallTileMap(int width, int height, float tileWidth, float tileHeight){}
        
        public void GenerateWallTileMap(int width, int height, float tileWidth, float tileHeight, MapData mapData){}
        
        
    }
}