using Leopotam.EcsLite;
using Game.Components;
using UnityEngine;
using Game.Data;
using Leopotam.EcsLite.Di;

namespace Game.Systems.Move
{
    public class MoveSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MoveComponent, UnitComponent>> _moveFilter = default;

        readonly float _moveSlowCoef = 2f;
        readonly float _pauseBeforeJump = 0.2f;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _moveFilter.Value)
            {
                ref var moveComponent = ref _moveFilter.Pools.Inc1.Get(entity);
                ref var unitComponent = ref _moveFilter.Pools.Inc2.Get(entity);

                Fly(ref unitComponent);
                Jump(ref moveComponent, ref unitComponent);
                Move(ref moveComponent, ref unitComponent);
            }
        }

        private void Move(ref MoveComponent moveComponent, ref UnitComponent unitComponent)
        {
            float moveDirection = moveComponent.moveDirection;
            float faceDirection = moveComponent.faceDirection;
            moveComponent.isMoving = moveDirection != 0;

            bool isForwardMove = (faceDirection > 0 && moveDirection > 0) ||
                (faceDirection < 0 && moveDirection < 0);
            bool isBackMove = (faceDirection > 0 && moveDirection < 0) ||
                (faceDirection < 0 && moveDirection > 0);

            unitComponent.animator.SetBool(AnimatorConstants.MOVE_FORWARD_BOOL, isForwardMove);
            unitComponent.animator.SetBool(AnimatorConstants.MOVE_BACK_BOOL, isBackMove);

            if (moveComponent.isMoving)
            {
                float currentSpeed = isBackMove || IsFlying(unitComponent) ?
                    moveComponent.moveSpeed / _moveSlowCoef : moveComponent.moveSpeed;

                float horizontalVelocity = moveDirection * currentSpeed;
                unitComponent.rb.linearVelocity = new Vector2(horizontalVelocity, unitComponent.rb.linearVelocity.y);
            }
        }

        private void Fly(ref UnitComponent unitComponent)
        {
            var isFlying = IsFlying(unitComponent);
            var canFlying = CanFly(ref unitComponent);

            if (!canFlying && isFlying)
            {
                unitComponent.animator.SetBool(AnimatorConstants.FLY_BOOL, false);
                unitComponent.animator.SetTrigger(AnimatorConstants.LANDING_TRIGGER);
            }
            else if(canFlying && !isFlying)
            {
                unitComponent.animator.SetBool(AnimatorConstants.FLY_BOOL, true);
                unitComponent.isJumped = false;
            }
        }

        private void Jump(ref MoveComponent moveComponent, ref UnitComponent unitComponent)
        {
            if (moveComponent.jumpButtonPressed && IsTouchingGround(unitComponent) 
                && !IsJumpAnimation(unitComponent))
            {
                unitComponent.animator.SetTrigger(AnimatorConstants.JUMP_TRIGGER);
            }

            if (IsJumpAnimation(unitComponent) && 
                unitComponent.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _pauseBeforeJump &&
                !unitComponent.isJumped)
            {
                unitComponent.rb.linearVelocity += Vector2.up * moveComponent.jumpPower;
                unitComponent.isJumped = true;
            }
        }

        private bool CanFly(ref UnitComponent unitComponent)
        {
            return !IsJumpAnimation(unitComponent) && !IsTouchingGround(unitComponent);
        }

        private bool IsTouchingGround(UnitComponent unitComponent)
        {
            return unitComponent.footCollider.IsTouchingLayers(LayerMask
                .GetMask("Ground", "Active Stuff", "Player"));
        }

        private bool IsJumpAnimation(UnitComponent unitComponent)
        {
            return unitComponent.animator.GetCurrentAnimatorStateInfo(0)
                .IsName(AnimatorConstants.JUMPING_ANIMATION_NAME);
        }

        private bool IsFlying(UnitComponent unitComponent)
        {
            return unitComponent.animator.GetBool(AnimatorConstants.FLY_BOOL); ;
        }
    }
}