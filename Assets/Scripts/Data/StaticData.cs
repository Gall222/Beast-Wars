using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu]
    public class StaticData : ScriptableObject
    {
        public WorldData worldData;
        public PlayerData playerData;
        //public SoundsData soundsData;

        //public Queue<EcsEntity> EnemiesEntities = new Queue<EcsEntity>();
    }

}