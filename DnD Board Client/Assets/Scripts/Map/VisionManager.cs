using System.Collections.Generic;
using Map.TileTypes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
 public class VisionManager : MonoBehaviour
    {
        private int visionRadiusTiles = 12;  // Radius in tile units (not world units)

        public static VisionManager Instance;

        private Tilemap visionTilemap;
        private CustomTileBase noVisionTile;
        private CustomTileBase fullVisionTile;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            visionTilemap = MapTileMapManager.MapTileMapManagerInstance.tileMaps["vision"];
            noVisionTile = TileGallery.TileGalleryInstance.GetTile("NoVision");
            fullVisionTile = TileGallery.TileGalleryInstance.GetTile("FullVision");
        }

        public void ClearVision(Vector3 worldPosition)
        {
            Vector3Int origin = visionTilemap.WorldToCell(worldPosition);
            int radiusSquared = visionRadiusTiles * visionRadiusTiles;

            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
            Queue<Vector3Int> queue = new Queue<Vector3Int>();

            queue.Enqueue(origin);
            visited.Add(origin);

            while (queue.Count > 0)
            {
                Vector3Int current = queue.Dequeue();

                if ((current - origin).sqrMagnitude > radiusSquared)
                    continue;

                if (visionTilemap.GetTile(current) == noVisionTile)
                {
                    visionTilemap.SetTile(current, fullVisionTile);
                }

                foreach (Vector3Int dir in Directions)
                {
                    Vector3Int neighbor = current + dir;

                    if (!visited.Contains(neighbor) &&
                        (neighbor - origin).sqrMagnitude <= radiusSquared)
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        // 8-directional movement for smooth vision circle
        private static readonly Vector3Int[] Directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(-1, -1, 0)
        };
    }
}