using Game.Components;
using Game.UI.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems.World
{
    public class ForegroundSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<SceneDataComponent> _sceneData = default;

        readonly EcsFilterInject<Inc<PlayerComponent>> _playerFilter = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                ref var playerComponent = ref _playerFilter.Pools.Inc1.Get(entity);
                if (!playerComponent.leftMouseDown) { return; }

                /** TODO только дл€ предметов, удал€ющих тайлы */
                TileDestroy();
            }
        }

        private void TileDestroy() 
        {
            var foregroundCollision = _sceneData.Value.foreground.IncomeCollision;

            if (foregroundCollision == null) { return; }

            if (foregroundCollision.gameObject.CompareTag("Digger"))
            {
                var tilemap = _sceneData.Value.foreground.Tilemap;
                Vector3 worldPosition = foregroundCollision.contacts[0].point;
                Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

                if (!tilemap.HasTile(cellPosition)) { return; }
              
                tilemap.SetTile(cellPosition, null);
            }
        }
    }
}