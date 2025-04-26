using System;
using System.Collections.Generic;


namespace Map.TileTypes.SerializableTiles
{
    [Serializable]
    public class SerializableVisionTile : SerializableCustomTile
    {
        public List<string> DiscoveredBy = new();
        
    }
}