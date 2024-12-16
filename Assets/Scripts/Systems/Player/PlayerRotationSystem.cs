using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Player
{
    public class PlayerRotationSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<MoveComponent, UnitComponent>> _rotateFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _rotateFilter.Value)
            {
                ref var moveComponent = ref _rotateFilter.Pools.Inc1.Get(entity);
                ref var unitComponent = ref _rotateFilter.Pools.Inc2.Get(entity);

                BodyRotate(ref moveComponent, ref unitComponent);
            }
        }
        private void BodyRotate(ref MoveComponent moveComponent, ref UnitComponent unitComponent)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var difference = mousePosition - unitComponent.transform.position;
            difference.Normalize();
            float faceDirection = Mathf.Sign(difference.x);
            //Debug.Log(difference);
            BodyFlip(ref unitComponent, faceDirection);
            moveComponent.faceDirection = faceDirection;
        }

        private void BodyFlip(ref UnitComponent unitComponent, float direction)
        {
            unitComponent.transform.localScale =
                new Vector2(direction * Mathf.Abs(unitComponent.transform.localScale.x),
                unitComponent.transform.localScale.y
                );
        }
    }
}