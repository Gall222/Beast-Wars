using Game.Components;
using Game.UI.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Systems.PlayerControl
{
    public class HandControlSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PlayerComponent, UnitComponent, MoveComponent>> _playerFilter = default;
        private Vector3 _initialWorldPosition = new Vector3();

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                ref var playerComponent = ref _playerFilter.Pools.Inc1.Get(entity);
                ref var unitComponent = ref _playerFilter.Pools.Inc2.Get(entity);

                ActiveTool ativeTool = playerComponent.currentTool.view;
                GameObject stuffPlace = playerComponent.view.StuffPlace;
                Rigidbody2D rb = playerComponent.currentTool.rb;
                Collider2D collider = playerComponent.currentTool.collider;

                _initialWorldPosition = stuffPlace.transform.position;

                if (playerComponent.leftMouseDown && !EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;

                    Vector3 direction = GetDirection(mousePosition - _initialWorldPosition, ref playerComponent);
                    Vector3 targetPosition = _initialWorldPosition + direction;
                    //Debug.Log(_initialWorldPosition);
                    if (ativeTool && ativeTool.IsTouchingAnotherObject)
                    {
                        rb.MovePosition(Vector3.Lerp(rb.position, _initialWorldPosition, 0.1f));
                    }
                    else
                    {
                        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition,
                    Time.deltaTime * playerComponent.handMoveSpeed));
                    }

                    RotateAroundPivot(direction, ref unitComponent, ref rb, _initialWorldPosition, playerComponent);
                    collider.enabled = true;
                }
                else
                {
                    collider.enabled = false;
                    rb.MovePosition(playerComponent.view.transform.TransformPoint(playerComponent.toolInitialPosition));
                    rb.MoveRotation(0);
                }
            }
        }

        private void SwitchToolCollider()
        {

        }

        private Vector3 GetDirection(Vector3 rawDirection, ref PlayerComponent playerComponent)
        {
            if (rawDirection.magnitude > playerComponent.maxHandDistance)
            {
                rawDirection.Normalize();
                rawDirection *= playerComponent.maxHandDistance;
            }
            return rawDirection;
        }

        private void RotateAroundPivot(Vector3 direction, ref UnitComponent unitComponent,
    ref Rigidbody2D rb, Vector3 pivotPoint, PlayerComponent playerComponent)
        {
            // Рассчитываем угол относительно точки вращения
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Корректировка угла для отражения персонажа
            angle += unitComponent.transform.localScale.x < 0 ? 150f : 30f;

            Vector3 newPosition = pivotPoint + direction.normalized *
                Mathf.Clamp(direction.magnitude, 0, playerComponent.maxHandDistance);

            rb.MoveRotation(angle);
            rb.MovePosition(newPosition);
        }
    }
}