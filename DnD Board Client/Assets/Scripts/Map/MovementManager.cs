using System;
using System.Collections.Generic;
using Map.TileTypes;
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
        private Tilemap _wallTilemap;

        void Awake()
        {
            Instance = this;

            
        }

        private void Start()
        {
            _groundTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["ground"];
            _overlayTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["overlay"];
            _wallTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["wall"];
        }

        public void HighlightTilesInRangeDijkstra(Vector3Int startPos, int maxCost)
        {
            var queue = new PriorityQueue<Vector3Int>();
            var graph = new Dictionary<Vector3Int, int>();
            
            queue.Enqueue(startPos, 0);
            graph[startPos] = 0;
            
            Vector3Int[] directions = new Vector3Int[]
            {
                Vector3Int.right,
                Vector3Int.left,
                Vector3Int.up,
                Vector3Int.down,
                //Diagonals, tr, tl, br, bl
                new Vector3Int(1, 1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(1, -1, 0),
                new Vector3Int(-1, -1, 0)
            };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var currentCost = graph[current];

                if (currentCost > maxCost)
                {
                    continue;
                }
                
                MapTileMapManager.MapTileMapManagerInstance.HighlightPossibleMovementPath(current);
                
                foreach (var direction in directions)
                {
                    var neighbour =  current + direction;

                    if (!IsValidGroundTile(current, neighbour))
                    {
                        continue;
                    }

                    FloorTile neighbourTile = _groundTilemap.GetTile(neighbour) as FloorTile;
                    var moveCost = neighbourTile.MovementCost;
                    Debug.Log($"movecost = {moveCost}");
                    var newCost = currentCost + moveCost;

                    if (newCost > maxCost)
                        continue;

                    if (!graph.ContainsKey(neighbour) || newCost < graph[neighbour])
                    {
                        graph[neighbour] = newCost;
                        queue.Enqueue(neighbour, newCost);
                    }
                }
            }
        }

        private bool IsValidGroundTile(Vector3Int from, Vector3Int to)
        {
            if (!_groundTilemap.HasTile(to))
            {
                return false;
            }

            if (IsDiagonal(from, to))
            {
                var stepX = new Vector3Int(to.x, from.y, 0);
                var stepY = new Vector3Int(from.x, to.y, 0);
                if (!IsValidGroundTile(from, stepX) || !IsValidGroundTile(from, stepY))
                    return false;
            }

            var worldMin = _groundTilemap.CellToWorld(to);
            var worldMax = worldMin + _groundTilemap.cellSize;
            var wallSize = _wallTilemap.cellSize.x;

            for (var x = worldMin.x; x < worldMax.x; x += wallSize)
            {
                for (float y = worldMin.y; y < worldMax.y; y += wallSize)
                {
                    var checkPos = new Vector3(x + wallSize / 2, y + wallSize / 2, 0);
                    var wallCell = _wallTilemap.WorldToCell(checkPos);
                    if (_wallTilemap.HasTile(wallCell))
                        return false;
                }
            }

            return true;
        }
        
        
        private bool IsDiagonal(Vector3Int from, Vector3Int to)
        {
            return from.x != to.x && from.y != to.y;
        }
    }
}