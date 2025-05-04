using System.Collections.Generic;
using System.Linq;
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
        }
        
        
        
        
        //Using Dijkstra's algorithm to check what tiles can be accessed
        public Dictionary<Vector3Int, int> GetReachableTiles(Vector3Int start, int maxCost)
        {
            //Stores locations nd their costs
            var costMap = new Dictionary<Vector3Int, int>();
            
            var visited = new HashSet<Vector3Int>();

            var priorityQueue = new PriorityQueue<Vector3Int>();
            priorityQueue.Enqueue(start, 0);
            costMap[start] = 0;

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();
                if (visited.Contains(current)) continue;

                visited.Add(current);
                int currentCost = costMap[current];

                if (!IsValidGroundTile(current))
                    continue;

                if (currentCost != 0)
                {
                    _overlayTilemap.SetTile(current, TileGallery.TileGalleryInstance.GetTile("MovementOverlay"));
                }

                foreach (Vector3Int neighbour in GetNeighbours(current))
                {
                    if (!IsValidGroundTile(neighbour))
                        continue;

                    if (IsDiagonal(current, neighbour))
                    {
                        var stepX = new Vector3Int(current.x, neighbour.y, 0);
                        var stepY = new Vector3Int(neighbour.x, current.y, 0);

                        if (!IsValidGroundTile(stepX) || !IsValidGroundTile(stepY))
                            continue;
                    }

                    int moveCost = _groundTilemap.GetTile<FloorTile>(neighbour).MovementCost;
                    int newCost = currentCost + moveCost;

                    if (newCost > maxCost)
                        continue;

                    // Only enqueue if the new cost is better or the tile hasn't been visited
                    if (!costMap.ContainsKey(neighbour) || newCost < costMap[neighbour])
                    {
                        costMap[neighbour] = newCost;
                        priorityQueue.Enqueue(neighbour, newCost);
                    }
                }
            }

            return costMap;
        }
        
        
        //Gets the positions of neighbours
        List<Vector3Int> GetNeighbours(Vector3Int position)
        {
            var directions = new List<Vector3Int>
            {
                new (1, 0, 0),
                new (-1, 0, 0),
                new (0, 1, 0),
                new (0, -1, 0),
                new (1, 1, 0),
                new (1, -1, 0),
                new (-1, 1, 0),
                new (-1, -1, 0)
            };

            return directions.Select(direction => position + direction).ToList();
        }
        
        //Checks to see if the tile is a valid ground tile to move to
        private bool IsValidGroundTile(Vector3Int tile)
        {
            //If out of bounds
            if (!_groundTilemap.HasTile(tile))
            {
                return false;
            }
            
            //Gets the bounds of the current tile in world space
            Bounds bounds = new Bounds(
                _groundTilemap.GetCellCenterWorld(tile),
                _groundTilemap.cellSize
            );
            
            //Fires a box off centred on the player.
            //Size uses the tile size
            var hits = Physics2D.OverlapBoxAll(
                bounds.center,
                new Vector2(MapTileMapManager.MapTileMapManagerInstance.tileHeight, MapTileMapManager.MapTileMapManagerInstance.tileWidth) *0.8f,
                0f,
                LayerMask.GetMask("Walls")
            );
            
            //If there's one or fewer wall tiles hit by the collider box, return true
            return hits.Length <= 1;
        }
        
        
        private bool IsDiagonal(Vector3Int from, Vector3Int to)
        {
            return from.x != to.x && from.y != to.y;
        }
        
        
        //Debug, yes I know this is chatgpt generated but it saved time
        public void DebugDrawTileCheck(Vector3Int tilePos)
        {
            Vector3 center = _groundTilemap.GetCellCenterWorld(tilePos);
            Vector2 size = new Vector2(MapTileMapManager.MapTileMapManagerInstance.tileHeight,
                MapTileMapManager.MapTileMapManagerInstance.tileWidth) *0.8f ;

            // Optional: Check how many colliders are inside
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, LayerMask.GetMask("Walls"));

            // Color depending on hit count
            Color boxColor = hits.Length > 1 ? Color.red : (hits.Length == 1 ? Color.yellow : Color.green);

            // Corners of the box
            Vector3 topLeft     = center + new Vector3(-size.x / 2, size.y / 2);
            Vector3 topRight    = center + new Vector3(size.x / 2, size.y / 2);
            Vector3 bottomLeft  = center + new Vector3(-size.x / 2, -size.y / 2);
            Vector3 bottomRight = center + new Vector3(size.x / 2, -size.y / 2);

            // Draw box
            Debug.DrawLine(topLeft, topRight, boxColor, 200f);
            Debug.DrawLine(topRight, bottomRight, boxColor, 200f);
            Debug.DrawLine(bottomRight, bottomLeft, boxColor, 200f);
            Debug.DrawLine(bottomLeft, topLeft, boxColor, 200f);

            // Optional: print hit info
            if (hits.Length > 0)
            {
                Debug.Log($"Tile {tilePos} has {hits.Length} collider(s)");
            }
        }
    }
}