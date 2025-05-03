using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public class MovementManager : MonoBehaviour
    {
        public static MovementManager Instance;
        private List<GameObject> _highlightedTiles = new();
        private Tilemap _groundTilemap;
        private Tilemap _overlayTilemap;

        void Awake()
        {
            Instance = this;
            _groundTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["GroundTileMap"]
                .GetComponent<Tilemap>();
            _overlayTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["OverlayTileMap"]
                .GetComponent<Tilemap>();
            
        }

        public void HighlightTilesInRangeDijkstra(Vector3Int startPos, int maxCost)
        {
            TileMapManager.Clear
        }
    }
}