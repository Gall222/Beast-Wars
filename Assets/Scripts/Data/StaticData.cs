using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game Data/Static Data")]
    public class StaticData : ScriptableObject
    {
        public WorldData worldData;
        public PlayerData playerData;
        public UIData UIData;
        public BuildingData buildingData;
        public TilemapData tilemapData;

        //public Queue<EcsEntity> EnemiesEntities = new Queue<EcsEntity>();
    }

}