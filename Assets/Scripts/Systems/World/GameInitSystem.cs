using Game.Data;
using Game.Management;
using Game.UI.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Game.Systems.World
{
    public class GameInitSystem : IEcsInitSystem
    {
        readonly EcsCustomInject<SceneDataComponent> _sceneData = default;
        readonly EcsCustomInject<StaticData> _staticData = default;
        readonly EcsCustomInject<ButtonManager> _buttonManager = default;

        private List<Vector2> emptyTiles = new List<Vector2>();

        public void Init(IEcsSystems systems)
        {
            CreateForeground();
            CreateBackground();
            CreateWorldFrame();

            _buttonManager.Value.CreateButton(ButtonDictionary.SkillTypes.Build, ButtonDictionary.SkillNames.Stone);
        }

        private void CreateForeground()
        {
            for (int x = 0; x < _sceneData.Value.horizontalSize; x++)
            {
                for (int y = 0; y < _sceneData.Value.verticalSize; y++)
                {
                    var newTileCoordinates = new Vector2(x, y);
                    //Если рядом есть пустоты, шанс пустого тайла выше
                    var emptyTileCreateChance = IsEmptyTilesNearExist(newTileCoordinates) ? _sceneData.Value.caveChance : _sceneData.Value.emptyTileChance;
                    //Если шанс на пустоту успешен, сохраняем ее координаты в список, иначе создаем тайл
                    if (Random.Range(0f, 100f) <= emptyTileCreateChance)
                    {
                        //Debug.Log(a +" is true");
                        emptyTiles.Add(newTileCoordinates);
                    }
                    else
                    {
                        _sceneData.Value.foreground.Tilemap.SetTile(new Vector3Int(x, y, 0), _sceneData.Value.groundTile);
                    }
                }
            }
        }
        private void CreateBackground()
        {
            for (int x = 0; x < _sceneData.Value.horizontalSize; x++)
            {
                for (int y = 0; y < _sceneData.Value.verticalSize; y++)
                {
                    _sceneData.Value.background.SetTile(new Vector3Int(x, y, 0), _sceneData.Value.backgroundTile);
                }
            }
        }
        private void CreateWorldFrame()
        {
            for (int x = -1; x < _sceneData.Value.horizontalSize; x++)
            {
                _sceneData.Value.foreground.Tilemap.SetTile(new Vector3Int(x, -1, 0), _sceneData.Value.groundTile);
                _sceneData.Value.foreground.Tilemap.SetTile(new Vector3Int(x, _sceneData.Value.verticalSize * 2, 0), _sceneData.Value.groundTile);
            }
            for (int y = -1; y < _sceneData.Value.verticalSize * 2; y++)
            {
                _sceneData.Value.foreground.Tilemap.SetTile(new Vector3Int(-1, y, 0), _sceneData.Value.groundTile);
                _sceneData.Value.foreground.Tilemap.SetTile(new Vector3Int(_sceneData.Value.horizontalSize, y, 0), _sceneData.Value.groundTile);
            }
        }
        private bool IsEmptyTilesNearExist(Vector2 tileCoordinates)
        {
            return emptyTiles.Contains(tileCoordinates + Vector2.left) ||
                emptyTiles.Contains(tileCoordinates + Vector2.right) ||
                emptyTiles.Contains(tileCoordinates + Vector2.up) ||
                emptyTiles.Contains(tileCoordinates + Vector2.down);
        }
    }
}