using Game.Components;
using Game.Management;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

namespace Game.Systems.ButtonControl
{
    public class BuildSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<BuildComponent>> _buildFilter = default;
        readonly EcsCustomInject<ButtonManager> _buttonManager = default;
        readonly EcsCustomInject<PlayerInput> _playerInput = default;
        readonly EcsPoolInject<BuildComponent> _buildComponentPool = default;


        private Dictionary<ColorNames, Color> _colors = new Dictionary<ColorNames, Color>() 
        {
            {ColorNames.Allowed, new Color(0.5f, 1f, 0.5f, 0.39f)},
            {ColorNames.Denied, new Color(1f, 0.5f, 0.5f, 0.39f)},
            {ColorNames.Builded, Color.white},
        };
        public enum ColorNames
        {
            Allowed,
            Denied,
            Builded
        }

        public void Run (IEcsSystems systems) {
            foreach (int entity in _buildFilter.Value)
            {
                ref BuildComponent buildComponent = ref _buildFilter.Pools.Inc1.Get(entity);
                //Debug.Log();
                if (_playerInput.Value.Player.RightClick.IsPressed()) { 
                    Cancel(ref buildComponent, entity);
                    return;
                }

                GameObject buildObject = buildComponent.gameObject;

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                
                buildObject.transform.position = new Vector3(
                    Mathf.RoundToInt(mousePosition.x),
                    Mathf.RoundToInt(mousePosition.y),
                    0);

                bool canBuild = !buildComponent.view.IsAcrossingSomething && buildComponent.view.IsOnTheBuildingArea;
                Debug.Log("Not across: " + !buildComponent.view.IsAcrossingSomething + 
                    ". On the build area: " + buildComponent.view.IsOnTheBuildingArea);
                buildComponent.sprite.color = canBuild ? _colors[ColorNames.Allowed] : _colors[ColorNames.Denied];

                if (_playerInput.Value.Player.Click.IsPressed() && canBuild)
                {
                    Build(ref buildComponent, entity);
                    return;
                }

                //unitComponent.footCollider.IsTouchingLayers(LayerMask
                //.GetMask("Ground", "Active Stuff", "Player"));
            }
        }

        public void Build(ref BuildComponent buildComponent, int entity)
        {
            buildComponent.sprite.color = _colors[ColorNames.Builded];
            buildComponent.view.InnerCollider.isTrigger = false;
            _buildComponentPool.Value.Del(entity);
            _buttonManager.Value.SetClick(ClickController.ClickTypes.None);
        }

        public void Cancel(ref BuildComponent buildComponent, int entity)
        {
            GameObject.Destroy(buildComponent.gameObject);
            _buildFilter.Value.GetWorld().DelEntity(entity);
            _buttonManager.Value.SetClick(ClickController.ClickTypes.None);
        }
    }
}