using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

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

                var hand = playerComponent.view.hand;
                var rb = hand.GetComponent<Rigidbody2D>();
                _initialWorldPosition = playerComponent.view.transform.
                    TransformPoint(playerComponent.handInitialPosition);

                if (playerComponent.leftMouseDown)
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;

                    var direction = GetDirection(mousePosition, ref playerComponent);

                    Vector3 targetPosition = _initialWorldPosition + direction;

                    rb.MovePosition(Vector3.Lerp(hand.transform.position, targetPosition, 
                        Time.deltaTime * playerComponent.handMoveSpeed));
                    Rotate(direction, ref unitComponent, ref rb);
                }
                else
                {
                    rb.MovePosition(_initialWorldPosition);
                    rb.MoveRotation(0);
                }
            }
        }

        private Vector3 GetDirection(Vector3 mousePosition, ref PlayerComponent playerComponent)
        {
            Vector3 direction = mousePosition - _initialWorldPosition;

            if (direction.magnitude > playerComponent.maxHandDistance)
            {
                direction.Normalize();
                direction *= playerComponent.maxHandDistance;
            }

            return direction;
        }

        private void Rotate(Vector3 direction, ref UnitComponent unitComponent, ref Rigidbody2D rb)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (unitComponent.transform.localScale.x < 0)
            {
                angle += 110f;
            }
            else
            {
                angle += 70f;
            }

            rb.MoveRotation(angle);
        }
    }
}