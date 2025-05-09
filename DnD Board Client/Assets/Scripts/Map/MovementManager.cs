using System.Collections.Generic;
using System.Linq;
using DataObjects.Units;
using DefaultNamespace.TurnBasedScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public class MovementManager : MonoBehaviour
    {
        public static MovementManager Instance;
        private List<Vector3Int> _highlightedTiles = new();
        private Tilemap _groundTilemap;
        private Tilemap _overlayTilemap;
        private Tilemap _wallTilemap;

        public GameObject forceMoveBtn;
        public GameObject cancelForceMoveBtn;

        private bool _forceMoveMode;
        
        void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _groundTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["ground"];
            _overlayTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["overlay"];
        }
        private Vector3Int _startingTile;
        private Dictionary<Vector3Int, int> _reachableTiles = new();
        private Vector3Int _currentTile;
        private bool _awaitingInput = false;

        private void Update()
        {
            if (!MapManager.MapManagerInstance.currentlySelectedUnit)
            {
                forceMoveBtn.SetActive(false);
                cancelForceMoveBtn.SetActive(false);
            }
            else
            {
                if (_forceMoveMode)
                {
                    forceMoveBtn.SetActive(false);
                    cancelForceMoveBtn.SetActive(true);

                    if (Input.GetMouseButtonDown(1))
                    {
                        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        var tileMapPos = MapTileMapManager.MapTileMapManagerInstance.tileMaps["ground"].WorldToCell(mouseWorldPos);
                        var center = MapTileMapManager.MapTileMapManagerInstance.tileMaps["ground"].GetCellCenterWorld(tileMapPos);
                        MapManager.MapManagerInstance.currentlySelectedUnit.transform.position = center;

                        if (TurnBasedModeManager.Instance.IsTurnBasedMode)
                        {
                            _currentTile = tileMapPos;
                            int remaining = TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed;
                            _reachableTiles = GetReachableTiles(_currentTile, remaining);
                        }
                    }
                }
                else
                {
                    forceMoveBtn.SetActive(true);
                    cancelForceMoveBtn.SetActive(false);
                }
            }
            
            
            if (!TurnBasedModeManager.Instance.IsTurnBasedMode)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickedTile = _groundTilemap.WorldToCell(mouseWorld);
                HighlightPathTile(clickedTile);
            }

            if (Input.GetMouseButtonUp(1))
            {
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickedTile = _groundTilemap.WorldToCell(mouseWorld);
                RemoveHighlightPathTile(clickedTile);
            }
        }

        public void EnableForceMoveMode()
        {
            _forceMoveMode = true;
        }

        public void DisableForceMoveMode()
        {
            _forceMoveMode = false;
        }
        public void ClearData()
        {
            _highlightedTiles.Clear();
            
        }

        public void BeginPlayerMovement(Vector3Int startTile)
        {
            _currentTile = startTile;
            _awaitingInput = true;
            _startingTile = startTile;

            var remaining = TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed;
            _reachableTiles = GetReachableTiles(_currentTile, remaining);
        }

        private void RemoveHighlightPathTile(Vector3Int clicked)
        {
            if (_highlightedTiles.Count == 0)
                return;

            Vector3Int lastTile = _highlightedTiles[^1]; // last tile in list

            if (clicked != lastTile)
            {
                Debug.Log("You can only undo the last tile in the path.");
                return;
            }

            RefundLastStep();
        }
        
        private void RefundLastStep()
        {
            if (_highlightedTiles.Count == 0)
                return;

            Vector3Int lastTile = _highlightedTiles[^1]; // get last tile
            _highlightedTiles.RemoveAt(_highlightedTiles.Count - 1); // remove from path

            int costToRefund = _groundTilemap.GetTile<FloorTile>(lastTile).MovementCost;
            TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed += costToRefund;
            Debug.Log($"Refunded {costToRefund} movement from tile {lastTile}");

            // Update current tile to new last tile, or starting tile if path is empty
            _currentTile = _highlightedTiles.Count > 0 ? _highlightedTiles[^1] : _startingTile;

            ClearOverlayTiles();

            // Recalculate reachable tiles from the new current tile
            if (TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed > 0)
            {
                _reachableTiles = GetReachableTiles(_currentTile, TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed);
            }

            // Re-highlight the path
            foreach (var tile in _highlightedTiles)
            {
                _overlayTilemap.SetTile(tile, TileGallery.TileGalleryInstance.GetTile("SelectedOverlay"));
            }
            
            MapManager.MapManagerInstance.MoveUnit(_overlayTilemap.GetCellCenterWorld(_currentTile), 
                TurnBasedModeManager.Instance.GetCurrentTurn());
        }
        private void HighlightPathTile(Vector3Int clicked)
        {
            if (!_reachableTiles.ContainsKey(clicked)) return;
            if (!GetNeighbours(_currentTile).Contains(clicked)) return;
            
            int moveCost = _reachableTiles[clicked];
            if (moveCost > TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed) return;

            var selectedTile = _groundTilemap.WorldToCell(clicked);
            _highlightedTiles.Add(clicked);

            TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed -= moveCost;
            _currentTile = clicked;

            // Recalculate remaining reachable tiles
            ClearOverlayTiles();
            if (TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed > 0)
            {
                _reachableTiles = GetReachableTiles(_currentTile, TurnBasedModeManager.Instance.UnitTurn.RemainingMovementSpeed);
            }
            
            foreach (var tile in _highlightedTiles)
            {
                _overlayTilemap.SetTile(tile, TileGallery.TileGalleryInstance.GetTile("SelectedOverlay"));
            }
            
      

            var temp = MapManager.MapManagerInstance.allInstntiatedUnits;
            foreach (var unitB in temp)
            {
                Debug.Log(unitB.Key);
            }
            
            MapManager.MapManagerInstance.MoveUnit(_overlayTilemap.GetCellCenterWorld(_currentTile), 
                TurnBasedModeManager.Instance.GetCurrentTurn());
            
        }
        
        public void ClearOverlayTiles()
        {
            _overlayTilemap.ClearAllTiles(); // Or only clear MovementOverlay tiles if needed
        }
        
        //Using Dijkstra's algorithm to check what tiles can be accessed
        private Dictionary<Vector3Int, int> GetReachableTiles(Vector3Int start, int maxCost)
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
                var currentCost = costMap[current];

                if (!IsValidGroundTile(current))
                    continue;

                if (currentCost != 0)
                {
                    if (!_overlayTilemap.HasTile(current))
                    {
                        _overlayTilemap.SetTile(current, TileGallery.TileGalleryInstance.GetTile("MovementOverlay"));
                    }
                    
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
                new Vector2(MapTileMapManager.MapTileMapManagerInstance.tileHeight, MapTileMapManager.MapTileMapManagerInstance.tileWidth) *0.65f,
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