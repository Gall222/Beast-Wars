using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public WorldData worldData;
        public PlayerData playerData;
        public UIData UIData;
        public BuildingData buildingData;

        //public Queue<EcsEntity> EnemiesEntities = new Queue<EcsEntity>();
    }

}