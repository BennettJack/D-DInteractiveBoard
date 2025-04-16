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
    }
}