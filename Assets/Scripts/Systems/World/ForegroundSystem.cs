using Game.Components;
using Game.Data;
using Game.UI.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Systems.World
{
    public class ForegroundSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<SceneDataComponent> _sceneData = default;
        readonly EcsCustomInject<StaticData> _staticData = default;
        readonly EcsFilterInject<Inc<PlayerComponent>> _playerFilter = default;

        private Dictionary<Vector3Int, int> _tileTouchCounter = new Dictionary<Vector3Int, int>();

        private int _firstDestructionStrikeCounts = 3;
        private int _secondDestructionStrikeCounts = 6;
        private int _thirdDestructionStrikeCounts = 9;

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
                Tilemap tilemap = _sceneData.Value.foreground.Tilemap;
                Tilemap effectsMap = _sceneData.Value.effectsMap;
                Vector3 worldPosition = foregroundCollision.contacts[0].point;
                Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

                if (!tilemap.HasTile(cellPosition)) { return; }

                IncraceTouchCount(cellPosition);
                AddDestructionEffect(cellPosition, tilemap);
            }
        }

        private void IncraceTouchCount(Vector3Int cellPosition)
        {
            if (!_tileTouchCounter.ContainsKey(cellPosition))
            {
                _tileTouchCounter[cellPosition] = 0;
            }
            _tileTouchCounter[cellPosition]++;
        }

        private void AddDestructionEffect(Vector3Int cellPosition, Tilemap tilemap)
        {
            var effectsMap = _sceneData.Value.effectsMap;
            if (_tileTouchCounter[cellPosition] == _firstDestructionStrikeCounts)
            {
                effectsMap.SetTile(cellPosition, _staticData.Value.tilemapData.lightDestuction);
            }
            else if (_tileTouchCounter[cellPosition] == _secondDestructionStrikeCounts)
            {
                effectsMap.SetTile(cellPosition, _staticData.Value.tilemapData.middleDestuction);
            }
            else if (_tileTouchCounter[cellPosition] >= _thirdDestructionStrikeCounts)
            {
                effectsMap.SetTile(cellPosition, null);
                tilemap.SetTile(cellPosition, null);
                _tileTouchCounter.Remove(cellPosition);
            }
        }
    }
}