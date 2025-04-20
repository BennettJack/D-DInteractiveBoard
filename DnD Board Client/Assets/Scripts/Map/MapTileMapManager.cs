using System;
using TileMapControl;

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
    }
}