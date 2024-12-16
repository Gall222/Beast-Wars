using Leopotam.EcsLite;
using Components;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Systems.Player
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<PlayerInput> _playerInput = default;
        
        private EcsFilterInject<Inc<PlayerComponent, MoveComponent>> _inputEventsFilter = default;

        public void Run(IEcsSystems systems)
        {
            var moveDirection = _playerInput.Value.Player.Move.ReadValue<Vector2>().x;

            foreach (var entity in _inputEventsFilter.Value)
            {
                ref var playerComponent = ref _inputEventsFilter.Pools.Inc1.Get(entity);
                ref var moveComponent = ref _inputEventsFilter.Pools.Inc2.Get(entity);

                AddDirection(ref moveComponent);
                IsJumping(ref moveComponent);
                IsLeftMouseDown(ref playerComponent);
            }
        }

        private void IsJumping(ref MoveComponent moveComponent)
        {
            moveComponent.jumpButtonPressed = _playerInput.Value.Player.Jump.IsPressed();
        }

        private void AddDirection(ref MoveComponent moveComponent)
        {
            moveComponent.moveDirection = _playerInput.Value.Player.Move.ReadValue<Vector2>().x;
        }

        private void IsLeftMouseDown(ref PlayerComponent playerComponent)
        {
            playerComponent.leftMouseDown = _playerInput.Value.Player.Click.IsPressed();
        }
    }
}