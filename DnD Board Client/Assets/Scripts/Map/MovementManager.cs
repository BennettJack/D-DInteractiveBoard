using System;
using System.Collections.Generic;
using System.Linq;
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

        public Dictionary<Vector3Int, int> GetReachableTiles(Vector3Int start, int maxCost)
        {
            var reachable = new Dictionary<Vector3Int, int>();
            var costSoFar = new Dictionary<Vector3Int, int>();
            var queue = new Queue<Vector3Int>();

            queue.Enqueue(start);
            costSoFar[start] = 0;

            while (queue.Count > 0)
            {
                Vector3Int current = queue.Dequeue();
                int currentCost = costSoFar[current];

                if (!IsValidGroundTile(start, current))
                    continue;

                reachable[current] = currentCost;
                _overlayTilemap.SetTile(current, TileGallery.TileGalleryInstance.GetTile("MovementOverlay"));

                foreach (Vector3Int neighbor in GetNeighbors(current))
                {
                    if (!IsValidGroundTile(current, neighbor))
                        continue;

                    int moveCost = _groundTilemap.GetTile<FloorTile>(neighbor).MovementCost;
                    int newCost = currentCost + moveCost;

                    if (newCost > maxCost)
                        continue;

                    if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                    {
                        costSoFar[neighbor] = newCost;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return reachable;
        }
        
        List<Vector3Int> GetNeighbors(Vector3Int pos)
        {
            var dirs = new List<Vector3Int>
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(0, -1, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(1, -1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(-1, -1, 0)
            };

            return dirs.Select(d => pos + d).ToList();
        }
        private bool IsValidGroundTile(Vector3Int from, Vector3Int to)
        {
            if (!_groundTilemap.HasTile(to))
            {
                Debug.Log("thi should never proc");
                return false;
            }

            if (IsDiagonal(from, to))
            {
                var stepX = new Vector3Int(to.x, from.y, 0);
                var stepY = new Vector3Int(from.x, to.y, 0);
                if (!IsValidGroundTile(from, stepX) && !IsValidGroundTile(from, stepY))
                {
                    Debug.Log($"Blocked diagonal from {from} to {to} — stepX and stepY both blocked.");
                    _overlayTilemap.SetTile(to, TileGallery.TileGalleryInstance.GetTile("BlockedOverlay"));
                    return false;
                }
            }

            Vector3 worldPos = _groundTilemap.GetCellCenterWorld(to);

// Calculate the 4 positions in wall tilemap space
            Vector3 halfCell = _groundTilemap.cellSize / 2f;
            Vector3[] corners = new Vector3[]
            {
                worldPos + new Vector3(-halfCell.x / 2f, -halfCell.y / 2f),
                worldPos + new Vector3(-halfCell.x / 2f,  halfCell.y / 2f),
                worldPos + new Vector3( halfCell.x / 2f, -halfCell.y / 2f),
                worldPos + new Vector3( halfCell.x / 2f,  halfCell.y / 2f),
            };

            int wallCount = 0;
            foreach (var corner in corners)
            {
                Vector3Int wallCell = _wallTilemap.WorldToCell(corner);
                if (_wallTilemap.HasTile(wallCell))
                {
                    wallCount++;
                    if (wallCount > 1)
                    {
                        Debug.Log($"Blocked {from} to {to} because 2+ wall tiles found in ground tile");
                        return false;
                    }
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