using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game Data/Tilemap Data")]
    public class TilemapData : ScriptableObject
    {
        public RuleTile groundTile;
        public RuleTile backgroundTile;
        public TileBase lightDestuction;
        public TileBase middleDestuction;
    }
}