using Leopotam.EcsLite;
using Game.Components;
using UnityEngine;
using Game.Data;
using Leopotam.EcsLite.Di;

namespace Game.Systems.Move
{
    public class MoveSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<StaticData> _staticData = default;
        readonly EcsFilterInject<Inc<MoveComponent, UnitComponent>> _moveFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _moveFilter.Value)
            {
                ref var moveComponent = ref _moveFilter.Pools.Inc1.Get(entity);
                ref var unitComponent = ref _moveFilter.Pools.Inc2.Get(entity);

                Jump(ref moveComponent, ref unitComponent);
                FrictionControl(ref moveComponent, ref unitComponent);
                Move(ref moveComponent, ref unitComponent);
            }
        }

        private void Move(ref MoveComponent moveComponent, ref UnitComponent unitComponent)
        {
            float moveDirection = moveComponent.moveDirection;
            float faceDirection = moveComponent.faceDirection;
            moveComponent.isMoving = moveDirection != 0;

            unitComponent.isForwardMove = (faceDirection > 0 && moveDirection > 0) ||
                (faceDirection < 0 && moveDirection < 0);
            unitComponent.isBackMove = (faceDirection > 0 && moveDirection < 0) ||
                (faceDirection < 0 && moveDirection > 0);

            if (!moveComponent.isMoving
                || Mathf.Abs(unitComponent.rb.linearVelocity.x) >= 10) { return; }

            unitComponent.rb.linearVelocity += new Vector2(moveDirection, 0);
        }

        private void Jump(ref MoveComponent moveComponent, ref UnitComponent unitComponent)
        {
            if (unitComponent.view.jumpTrigger)
            {
                if (unitComponent.isTouchingGround)
                {
                    unitComponent.rb.linearVelocity += Vector2.up * unitComponent.jumpPower;
                }
                unitComponent.view.jumpTrigger = false;
                unitComponent.state = UnitComponent.states.Jumping;
            }
        }

        private void FrictionControl(ref MoveComponent moveComponent, ref UnitComponent unitComponent)
        {
            unitComponent.bodyCollider.sharedMaterial = unitComponent.isTouchingGround
                ? _staticData.Value.playerData.normalFrictionMaterial
                : _staticData.Value.playerData.noFrictionMaterial;
        }
    }
}