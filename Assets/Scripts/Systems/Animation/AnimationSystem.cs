using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Game.Systems;
using UnityEngine;
using System;
using Game.Data;

namespace Game.Systems.Animation
{
    public class AnimationSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UnitComponent>> _unitFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _unitFilter.Value)
            {
                ref var unitComponent = ref _unitFilter.Pools.Inc1.Get(entity);

                Moving(ref unitComponent);
                Fly(ref unitComponent);
                Jump(ref unitComponent);
                Landing(ref unitComponent);
                Idle(ref unitComponent);
            }
        }

        private void Moving(ref UnitComponent unitComponent)
        {
            unitComponent.animator.SetBool(AnimatorConstants.MOVE_FORWARD_BOOL, unitComponent.isForwardMove);
            unitComponent.animator.SetBool(AnimatorConstants.MOVE_BACK_BOOL, unitComponent.isBackMove);
        }

        private void Jump(ref UnitComponent unitComponent)
        {
            if (unitComponent.jumpButtonPressed && unitComponent.isTouchingGround
                && CanJump(unitComponent.state))
            {
                unitComponent.animator.SetTrigger(AnimatorConstants.JUMP_TRIGGER);
                unitComponent.state = UnitComponent.states.PrepareJump;
            }
        }

        private void Fly(ref UnitComponent unitComponent)
        {
            if (!unitComponent.isTouchingGround && CanFly(unitComponent.state))
            {
                unitComponent.animator.SetBool(AnimatorConstants.FLY_BOOL, true);
                unitComponent.state = UnitComponent.states.Flying;
            }
            if (unitComponent.view.flyTrigger)
            {
                unitComponent.view.flyTrigger = false;
                unitComponent.state = UnitComponent.states.Flying;
            }
        }

        private void Landing(ref UnitComponent unitComponent)
        {

            if (unitComponent.isTouchingGround && unitComponent.state == UnitComponent.states.Flying)
            {
                unitComponent.animator.SetTrigger(AnimatorConstants.LANDING_TRIGGER);
                unitComponent.state = UnitComponent.states.Landing;
            }
        }

        private void Idle(ref UnitComponent unitComponent)
        {
            if (unitComponent.view.idleTrigger)
            {
                unitComponent.state = UnitComponent.states.Idle;
                unitComponent.view.idleTrigger = false;
                unitComponent.animator.SetBool(AnimatorConstants.FLY_BOOL, false);
            }
        }

        private bool CanJump(UnitComponent.states state)
        {
            switch (state)
            {
                case UnitComponent.states.Idle:
                case UnitComponent.states.BackMoving:
                case UnitComponent.states.Moving: return true;
                default: return false;
            }
        }

        private bool CanFly(UnitComponent.states state)
        {
            switch (state)
            {
                case UnitComponent.states.Flying:
                case UnitComponent.states.Landing: return false;
                default: return true;
            }
        }
    }
}