using Leopotam.EcsLite;
using Game.Components;
using UnityEngine;
using Leopotam.EcsLite.Di;
using System;

namespace Game.Systems.PlayerControl
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<PlayerInput> _playerInput = default;
        
        private EcsFilterInject<Inc<PlayerComponent, MoveComponent, UnitComponent>> _inputEventsFilter = default;

        public void Run(IEcsSystems systems)
        {
            var moveDirection = _playerInput.Value.Player.Move.ReadValue<Vector2>().x;

            foreach (var entity in _inputEventsFilter.Value)
            {
                ref var playerComponent = ref _inputEventsFilter.Pools.Inc1.Get(entity);
                ref var moveComponent = ref _inputEventsFilter.Pools.Inc2.Get(entity);
                ref var unitComponent = ref _inputEventsFilter.Pools.Inc3.Get(entity);

                AddDirection(ref moveComponent);
                IsJumping(ref unitComponent);
                IsTouchingGround(ref unitComponent);
                IsLeftMouseDown(ref playerComponent);
            }
        }

        private void IsTouchingGround(ref UnitComponent unitComponent)
        {
            unitComponent.isTouchingGround = unitComponent.footCollider.IsTouchingLayers(LayerMask
                .GetMask("Ground", "Active Stuff", "Player", "Building"));
        }

        private void IsJumping(ref UnitComponent unitComponent)
        {
            unitComponent.jumpButtonPressed = _playerInput.Value.Player.Jump.IsPressed();
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