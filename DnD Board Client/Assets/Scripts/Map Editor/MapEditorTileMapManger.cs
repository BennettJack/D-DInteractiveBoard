using TileMapControl;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map_Editor
{
    public class MapEditorTileMapManger : TileMapManager
    {
        public static MapEditorTileMapManger MapEditorTileMapMangerInstance;

        private void Awake()
        {
            MapEditorTileMapMangerInstance = this;
            
            AddTileMapsToDictionary();
            tileMaps.Add("preview", GameObject.FindGameObjectWithTag("EditorPreviewTileMap").GetComponent<Tilemap>());
        }
        
        void Start()
        {
            tileMaps["preview"].color = new Color(255, 255, 255, 0.2f);
            _tileGallery = TileGallery.TileGalleryInstance;
            _tileMapGenerator = TileMapGenerator.TileMapGeneratorInstance;

        }
        
        public void SetCurrentTileMapToEdit(string tileMapName)
        {
            foreach (var tileMap in tileMaps)
            {
                if (tileMap.Key == tileMapName)
                {
                    tileMap.Value.transform.parent.gameObject.SetActive(true);
                    tileMap.Value.color = new Color(255, 255, 255, 0.2f);
                }
                else
                {
                    tileMap.Value.transform.parent.gameObject.SetActive(false);
                }
            }
        }
        
        //Sets the amount of tiles that make up the map grid, vertically and horizontally
        public void SetTileCounts(int vTileCount, int hTileCount)
        {
            verticalTileCount = vTileCount;
            horizontalTileCount = hTileCount;
            foreach (var tileMap in tileMaps)
            {
                tileMap.Value.ClearAllTiles();
            }
            CreateNewTileSet("preview");
        }
        
        //Sets the width of each tile
        public void SetTileWidth(float tWidth)
        {
            tileWidth = tWidth;
            UpdateTileSize();
        }

        //Sets the height of each tile
        public void SetTileHeight(float tHeight)
        {
            tileHeight = tHeight;
            UpdateTileSize();
        }
    }
}